using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Messages;

namespace Identity.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBus _bus;
    private readonly IAuthDbContext _authDbContext;
    private readonly IConfiguration _config;

    public UserService(UserManager<AppUser> userManager, IBus bus, IAuthDbContext authDbContext,
        IConfiguration config)
    {
        _userManager = userManager;
        _bus = bus;
        _authDbContext = authDbContext;
        _config = config;
    }

    public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
    {
        var appUser = appUserDto.Adapt<AppUser>();

        await using var transaction = await _authDbContext.Database.BeginTransactionAsync();
        try
        {
            var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);

            if (!identityResult.Succeeded)
            {
                return new ServiceResult<int>()
                {
                    Value = 0,
                    Errors = new List<ServiceError>
                    {
                        new(ServiceErrorStatusCode.WrongAction,
                            identityResult.Errors.ToList()[0].Description)
                    }
                };
            }

            var message = appUser.Adapt<UserCreatedMessage>();
            
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationTime = int.Parse(_config["RabbitMQ:ConnectionRetryTimeInSeconds"]);
            if (cancellationTime == 0)
            {
                throw new ConfigurationException("RabbitMQ connection retry time limit is invalid");
            }
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(cancellationTime));
            
            await _bus.Publish<UserCreatedMessage>(message, cancellationTokenSource.Token);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new ServiceResult<int>()
        {
            Value = appUser.Id
        };
    }

    public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return new ServiceResult<AppUserDto>()
            {
                Errors = new List<ServiceError>
                {
                    new ServiceError(ServiceErrorStatusCode.NotFound, "User is not found")
                }
            };
        }
        var userDto = user.Adapt<AppUserDto>();
        return new ServiceResult<AppUserDto>()
        {
            Value = userDto
        };
    }

    public async Task<ServiceResult<List<AppUserDto>>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersDto = users.Adapt<List<AppUserDto>>();
        return new ServiceResult<List<AppUserDto>>()
        {
            Value = usersDto
        };
    }
}
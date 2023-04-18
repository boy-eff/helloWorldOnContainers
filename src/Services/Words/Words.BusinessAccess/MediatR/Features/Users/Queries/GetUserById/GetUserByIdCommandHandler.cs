using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.User;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Users.Queries.GetUserById;

public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, UserResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<GetUserByIdCommandHandler> _logger;

    public GetUserByIdCommandHandler(WordsDbContext dbContext, ILogger<GetUserByIdCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<UserResponseDto> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(request.UserId);

        if (user is null)
        {
            _logger.LogInformation("User with id {UserId} was not found", request.UserId);
            throw new NotFoundException("User was not found");
        }
        
        return user.Adapt<UserResponseDto>();
    }
}
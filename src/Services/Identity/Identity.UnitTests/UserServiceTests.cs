using FluentAssertions;
using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Identity.UnitTests;

public class UserServiceTests
{
    private Mock<UserManager<AppUser>> _userManagerMock;
    private Mock<IPublishEndpoint> _publishEndpointMock;
    private Mock<ILogger<UserService>> _loggerMock;
    private Mock<RoleManager<IdentityRole<int>>> _roleManagerMock;
    private Mock<IDbContext> _dbContextMock;
    private UserService _sut;
    
    [SetUp]
    public void Setup()
    {
        _userManagerMock = MockUserManager();
        _roleManagerMock = MockRoleManager();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _dbContextMock = new Mock<IDbContext>();
        _sut = new UserService(_userManagerMock.Object, _publishEndpointMock.Object, _loggerMock.Object, _roleManagerMock.Object, _dbContextMock.Object);
    }
    
    [Test]
    public async Task AddUserAsync_WhenSucceed_ShouldReturnUserIdWithoutErrors()
    {
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };  
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _sut.AddUserAsync(appUserRegisterDto);

        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task AddUserAsync_WhenUserExists_ShouldReturnWrongActionError()
    {
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = "User already exists" }));

        var result = await _sut.AddUserAsync(appUserRegisterDto);

        result.Succeeded.Should().BeFalse();
        result.Errors[0].StatusCode.Should().Be(ServiceErrorStatusCode.WrongAction);
    }
    
    [Test]
    public async Task GetUsersAsync_WhenSucceed_ShouldReturnUsers()
    {
        var appUser = new AppUser()
        {
            Id = 1,
            UserName = "username"
        };
        
        var users = new List<AppUser>()
        {
            appUser
        };
        
        _userManagerMock.Setup(x => x.Users)
            .Returns(users.AsQueryable().BuildMock());

        var result = await _sut.GetUsersAsync();

        result.Succeeded.Should().BeTrue();
        result.Value.Count.Should().Be(users.Count);
        var resultUserDto = result.Value.First();
        resultUserDto.Id.Should().Be(appUser.Id);
        resultUserDto.UserName.Should().Be(appUser.UserName);
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        var appUser = new AppUser()
        {
            Id = 1,
            UserName = "username"
        };
        var expectedAppUserDto = new AppUserDto() { Id = 1, UserName = "username" };
        _userManagerMock.Setup(x => x.FindByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);

        var result = await _sut.GetUserByIdAsync(appUser.Id);
        
        result.Succeeded.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedAppUserDto);
    }
    
    [Test]
    public async Task GetUserByIdAsync_WhenUserIsNotFound_ShouldReturn404NotFound()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<AppUser>(null));

        var result = await _sut.GetUserByIdAsync(0);
        
        result.Succeeded.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].StatusCode.Should().Be(ServiceErrorStatusCode.NotFound);
    }
    
    private static Mock<UserManager<AppUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<AppUser>>();
        var mgr = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<AppUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<AppUser>());
        return mgr;
    }
    
    private static Mock<RoleManager<IdentityRole<int>>> MockRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole<int>>>();
        
        var mgr = new Mock<RoleManager<IdentityRole<int>>>(store.Object, null, null, null, null);
        return mgr;
    }
}
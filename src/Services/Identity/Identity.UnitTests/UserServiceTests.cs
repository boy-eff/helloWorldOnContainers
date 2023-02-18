using FluentAssertions;
using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.WebAPI.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;

namespace Identity.UnitTests;

public class UserServiceTests
{
    [Test]
    public async Task AddUserAsync_WhenSucceed_ShouldReturnUserIdWithoutErrors()
    {
        var userManagerMock = MockUserManager();
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        var sut = new UserService(userManagerMock.Object);

        var result = await sut.AddUserAsync(appUserRegisterDto);

        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task AddUserAsync_WhenUserExists_ShouldReturnWrongActionError()
    {
        var userManagerMock = MockUserManager();
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = "User already exists" }));
        var sut = new UserService(userManagerMock.Object);

        var result = await sut.AddUserAsync(appUserRegisterDto);

        result.Succeeded.Should().BeFalse();
        result.Errors[0].StatusCode.Should().Be(ServiceErrorStatusCode.WrongAction);
    }
    
    [Test]
    public async Task GetUsersAsync_WhenSucceed_ShouldReturnUsers()
    {
        var userManagerMock = MockUserManager();
        
        var appUser = new AppUser()
        {
            Id = 1,
            UserName = "username"
        };
        
        var users = new List<AppUser>()
        {
            appUser
        };
        
        userManagerMock.Setup(x => x.Users)
            .Returns(users.AsQueryable().BuildMock());
        var sut = new UserService(userManagerMock.Object);

        var result = await sut.GetUsersAsync();

        result.Succeeded.Should().BeTrue();
        result.Value.Count.Should().Be(users.Count);
        var resultUserDto = result.Value.First();
        resultUserDto.Id.Should().Be(appUser.Id);
        resultUserDto.UserName.Should().Be(appUser.UserName);
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        var userManagerMock = MockUserManager();
        var appUser = new AppUser()
        {
            Id = 1,
            UserName = "username"
        };
        var expectedAppUserDto = new AppUserDto() { Id = 1, UserName = "username" };
        userManagerMock.Setup(x => x.FindByIdAsync(appUser.Id.ToString()))
            .ReturnsAsync(appUser);
        var sut = new UserService(userManagerMock.Object);

        var result = await sut.GetUserByIdAsync(appUser.Id);
        
        result.Succeeded.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedAppUserDto);
    }
    
    [Test]
    public async Task GetUserByIdAsync_WhenUserIsNotFound_ShouldReturn404NotFound()
    {
        var userManagerMock = MockUserManager();
        userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<AppUser>(null));
        var sut = new UserService(userManagerMock.Object);

        var result = await sut.GetUserByIdAsync(0);
        
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
}
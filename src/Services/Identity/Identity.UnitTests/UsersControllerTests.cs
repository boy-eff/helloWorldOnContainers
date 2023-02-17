using FluentAssertions;
using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.UnitTests;

public class UsersControllerTests
{
    [Test]
    public async Task AddUserAsync_WhenCredentialsAreValid_ShouldReturn200Ok()
    {
        var userServiceMock = new Mock<IUserService>();
        var createdUserId = 1;
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        var serviceResult = new ServiceResult<int>()
        {
            Value = createdUserId
        };
        userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<AppUserRegisterDto>()))
            .ReturnsAsync(serviceResult);
        var sut = new UsersController(userServiceMock.Object);

        var result = await sut.AddUserAsync(appUserRegisterDto);

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(createdUserId);
    }
    
    [Test]
    public async Task AddUserAsync_WhenUserExists_ShouldReturn400BadRequest()
    {
        var userServiceMock = new Mock<IUserService>();
        var createdUserId = 1;
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        var serviceResult = new ServiceResult<int>()
        {
            Errors = new List<ServiceError>()
            {
                new ServiceError(ServiceErrorStatusCode.WrongAction, "User already exists")
            },
            Value = createdUserId
        };
        userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<AppUserRegisterDto>()))
            .ReturnsAsync(serviceResult);
        var sut = new UsersController(userServiceMock.Object);

        var result = await sut.AddUserAsync(appUserRegisterDto);

        result.Should().BeAssignableTo<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(serviceResult.Errors[0].Message);
    }

    [Test]
    public async Task GetUsersAsync_ShouldReturn200Ok()
    {
        var userServiceMock = new Mock<IUserService>();
        var expectedAppUserDto = new AppUserDto() { Id = 1, UserName = "username" };
        var serviceResult = new ServiceResult<List<AppUserDto>>()
        {
            Value = new List<AppUserDto>()
            {
                expectedAppUserDto
            }
        };
        userServiceMock.Setup(x => x.GetUsersAsync())
            .ReturnsAsync(serviceResult);
        var sut = new UsersController(userServiceMock.Object);

        var result = await sut.GetUsersAsync();

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<List<AppUserDto>>()
                .Which.Count.Should().Be(1);

        result.As<OkObjectResult>().Value.As<List<AppUserDto>>()[0].Should().BeEquivalentTo(expectedAppUserDto);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturn200Ok()
    {
        var userServiceMock = new Mock<IUserService>();
        var id = 1;
        var expectedAppUserDto = new AppUserDto() { Id = id, UserName = "username" };
        var serviceResult = new ServiceResult<AppUserDto>()
        {
            Value = expectedAppUserDto
        };
        userServiceMock.Setup(x => x.GetUserByIdAsync(id))
            .ReturnsAsync(serviceResult);
        var sut = new UsersController(userServiceMock.Object);

        var result = await sut.GetUserByIdAsync(id);

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<AppUserDto>()
            .Which.Should().Be(expectedAppUserDto);
    }
    
    [Test]
    public async Task GetUserByIdAsync_WhenUserIsNotFound_ShouldReturn404NotFound()
    {
        var userServiceMock = new Mock<IUserService>();
        var expectedServiceError = new ServiceError(ServiceErrorStatusCode.NotFound, "User is not found");
        var serviceResult = new ServiceResult<AppUserDto>()
        {
            Errors = new List<ServiceError>() { expectedServiceError }
        };
        userServiceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(serviceResult);
        var sut = new UsersController(userServiceMock.Object);

        var result = await sut.GetUserByIdAsync(0);

        result.Should().BeAssignableTo<NotFoundObjectResult>()
            .Which.Value.Should().Be(expectedServiceError.Message);
    }
}
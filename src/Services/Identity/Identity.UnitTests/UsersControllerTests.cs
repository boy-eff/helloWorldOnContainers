using FluentAssertions;
using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.UnitTests;

public class UsersControllerTests
{
    private UsersController _sut;
    private Mock<IUserService> _userServiceMock;
    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _sut = new UsersController(_userServiceMock.Object);
    }
    [Test]
    public async Task AddUserAsync_WhenCredentialsAreValid_ShouldReturn200Ok()
    {
        const int createdUserId = 1;
        var appUserRegisterDto = new AppUserRegisterDto()
        {
            UserName = "username",
            Password = "password"
        };
        var serviceResult = new ServiceResult<int>()
        {
            Value = createdUserId
        };
        _userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<AppUserRegisterDto>()))
            .ReturnsAsync(serviceResult);

        var result = await _sut.AddUserAsync(appUserRegisterDto);

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(createdUserId);
    }
    
    [Test]
    public async Task AddUserAsync_WhenUserExists_ShouldReturn400BadRequest()
    {
        const int createdUserId = 1;
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
        _userServiceMock.Setup(x => x.AddUserAsync(It.IsAny<AppUserRegisterDto>()))
            .ReturnsAsync(serviceResult);

        var result = await _sut.AddUserAsync(appUserRegisterDto);

        result.Should().BeAssignableTo<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(serviceResult.Errors[0].Message);
    }

    [Test]
    public async Task GetUsersAsync_ShouldReturn200Ok()
    {
        var expectedAppUserDto = new AppUserDto() { Id = 1, UserName = "username" };
        var serviceResult = new ServiceResult<List<AppUserDto>>()
        {
            Value = new List<AppUserDto>()
            {
                expectedAppUserDto
            }
        };
        _userServiceMock.Setup(x => x.GetUsersAsync())
            .ReturnsAsync(serviceResult);

        var result = await _sut.GetUsersAsync();

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<List<AppUserDto>>()
                .Which.Count.Should().Be(1);

        result.As<OkObjectResult>().Value.As<List<AppUserDto>>()[0].Should().BeEquivalentTo(expectedAppUserDto);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturn200Ok()
    {
        const int id = 1;
        var expectedAppUserDto = new AppUserDto() { Id = id, UserName = "username" };
        var serviceResult = new ServiceResult<AppUserDto>()
        {
            Value = expectedAppUserDto
        };
        _userServiceMock.Setup(x => x.GetUserByIdAsync(id))
            .ReturnsAsync(serviceResult);

        var result = await _sut.GetUserByIdAsync(id);

        result.Should().BeAssignableTo<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<AppUserDto>()
            .Which.Should().Be(expectedAppUserDto);
    }
    
    [Test]
    public async Task GetUserByIdAsync_WhenUserIsNotFound_ShouldReturn404NotFound()
    {
        var expectedServiceError = new ServiceError(ServiceErrorStatusCode.NotFound, "User is not found");
        var serviceResult = new ServiceResult<AppUserDto>()
        {
            Errors = new List<ServiceError>() { expectedServiceError }
        };
        _userServiceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(serviceResult);

        var result = await _sut.GetUserByIdAsync(0);

        result.Should().BeAssignableTo<NotFoundObjectResult>()
            .Which.Value.Should().Be(expectedServiceError.Message);
    }
}
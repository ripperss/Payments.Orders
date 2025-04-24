using AtuhTests.Common;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NPOI.SS.Formula.Functions;
using PayMent.Orders.Domain.Data;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Auth;
using PayMents.Orders.Application.Service;
using PayMents.Orders.Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTest;

public class AuthTest
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<UserManager<UserIdentity>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<OrdersDbContext> _dbContextMock = new();
    private readonly IAuthService _authService;
    private readonly AppSettings _appSettings = new() 
    { 
        TokenPrivateKey = "test-key-1234567890-1234567890",
        Expires = 60
    };

    public AuthTest()
    {
        // Mock для UserManager
        var store = new Mock<IUserStore<UserIdentity>>();
        _userManagerMock = new Mock<UserManager<UserIdentity>>(
            store.Object, null, null, null, null, null, null, null, null);

        // Настройка сервиса
        _authService = new AuthService(
            Options.Create(_appSettings),
            _userManagerMock.Object,
            _mapperMock.Object,
            _emailServiceMock.Object,
            _dbContextMock.Object);
    }

    [Fact]
    public async Task RegisterAccount_ShouldReturnUserResponse_WhenRegistrationSuccessful()
    {
        // Arrange
        var userRegisterDto = userRegisterDtoFactory.CreateNoCorrectUserRegisterDto();

        var userIdentity = new UserIdentity { Email = userRegisterDto.Email, UserName = userRegisterDto.UserName };
        var userResponse = new UserResponse { Email = userRegisterDto.Email, UserName = userRegisterDto.UserName };

        _userManagerMock.Setup(x => x.FindByEmailAsync(userRegisterDto.Email))
            .ReturnsAsync((UserIdentity)null!);

        _mapperMock.Setup(x => x.Map<UserIdentity>(userRegisterDto))
            .Returns(userIdentity);

        _mapperMock.Setup(x => x.Map<UserResponse>(userIdentity))
            .Returns(userResponse);

        _userManagerMock.Setup(x => x.CreateAsync(userIdentity, userRegisterDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(userIdentity, Role.User))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.Register(userRegisterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userRegisterDto.Email, result.Email);
        Assert.Equal(userRegisterDto.UserName, result.UserName);
        Assert.Contains(Role.User, result.Roles);

        _userManagerMock.Verify(x => x.CreateAsync(userIdentity, userRegisterDto.Password), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(userIdentity, Role.User), Times.Once);
    }

}


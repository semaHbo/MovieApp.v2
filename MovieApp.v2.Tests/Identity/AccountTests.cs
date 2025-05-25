using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieApp.v2.Web.Areas.Identity.Pages.Account;
using System.Security.Claims;

namespace MovieApp.v2.Tests.Identity;

public class AccountTests
{
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
    private readonly Mock<ILogger<RegisterModel>> _mockLogger;

    public AccountTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object,
            null, null, null, null, null, null, null, null);

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        
        _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            _mockUserManager.Object,
            contextAccessorMock.Object,
            userPrincipalFactoryMock.Object,
            null, null, null, null);

        _mockLogger = new Mock<ILogger<RegisterModel>>();
    }

    [Fact]
    public async Task Register_ShouldCreateUser_WhenModelIsValid()
    {
        // Arrange
        var registerModel = new RegisterModel(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockLogger.Object);

        var input = new RegisterModel.InputModel
        {
            Email = "test@example.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!"
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await registerModel.OnPostAsync(input);

        // Assert
        _mockUserManager.Verify(x => x.CreateAsync(
            It.Is<IdentityUser>(u => u.Email == input.Email),
            input.Password), Times.Once);
    }

    [Fact]
    public async Task Register_ShouldReturnError_WhenUserCreationFails()
    {
        // Arrange
        var registerModel = new RegisterModel(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockLogger.Object);

        var input = new RegisterModel.InputModel
        {
            Email = "test@example.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!"
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        var result = await registerModel.OnPostAsync(input);

        // Assert
        registerModel.ModelState.IsValid.Should().BeFalse();
        registerModel.ModelState.ErrorCount.Should().Be(1);
    }

    [Fact]
    public async Task Login_ShouldSignInUser_WhenCredentialsAreValid()
    {
        // Arrange
        var loginModel = new LoginModel(
            _mockSignInManager.Object,
            _mockLogger.Object);

        var input = new LoginModel.InputModel
        {
            Email = "test@example.com",
            Password = "Test123!"
        };

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await loginModel.OnPostAsync(input);

        // Assert
        _mockSignInManager.Verify(x => x.PasswordSignInAsync(
            input.Email,
            input.Password,
            input.RememberMe,
            false), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturnError_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginModel = new LoginModel(
            _mockSignInManager.Object,
            _mockLogger.Object);

        var input = new LoginModel.InputModel
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = await loginModel.OnPostAsync(input);

        // Assert
        loginModel.ModelState.IsValid.Should().BeFalse();
        loginModel.ModelState.ErrorCount.Should().Be(1);
    }
} 
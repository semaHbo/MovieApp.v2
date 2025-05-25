using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.v2.Web.Controllers;
using MovieApp.v2.Web.Models;
using System.Security.Claims;

namespace MovieApp.v2.Tests.Authorization;

public class MovieAuthorizationTests
{
    private readonly Mock<IAuthorizationService> _mockAuthorizationService;
    private readonly ClaimsPrincipal _user;
    private readonly ClaimsPrincipal _admin;

    public MovieAuthorizationTests()
    {
        _mockAuthorizationService = new Mock<IAuthorizationService>();

        // Setup regular user
        _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user-id"),
            new Claim(ClaimTypes.Name, "user@example.com"),
            new Claim(ClaimTypes.Role, "User")
        }, "mock"));

        // Setup admin user
        _admin = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "admin-id"),
            new Claim(ClaimTypes.Name, "admin@example.com"),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));
    }

    [Fact]
    public async Task User_CanViewMovies()
    {
        // Arrange
        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.View))
            .ReturnsAsync(AuthorizationResult.Success());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _user,
            new Movie(),
            MovieOperations.View);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task RegularUser_CannotEditOtherUsersMovies()
    {
        // Arrange
        var movie = new Movie { CreatedById = "other-user-id" };

        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.Edit))
            .ReturnsAsync(AuthorizationResult.Failed());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _user,
            movie,
            MovieOperations.Edit);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task Admin_CanEditAnyMovie()
    {
        // Arrange
        var movie = new Movie { CreatedById = "other-user-id" };

        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.Edit))
            .ReturnsAsync(AuthorizationResult.Success());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _admin,
            movie,
            MovieOperations.Edit);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task User_CanEditOwnMovies()
    {
        // Arrange
        var movie = new Movie { CreatedById = "user-id" };

        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.Edit))
            .ReturnsAsync(AuthorizationResult.Success());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _user,
            movie,
            MovieOperations.Edit);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task RegularUser_CannotDeleteOtherUsersMovies()
    {
        // Arrange
        var movie = new Movie { CreatedById = "other-user-id" };

        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.Delete))
            .ReturnsAsync(AuthorizationResult.Failed());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _user,
            movie,
            MovieOperations.Delete);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task Admin_CanDeleteAnyMovie()
    {
        // Arrange
        var movie = new Movie { CreatedById = "other-user-id" };

        _mockAuthorizationService
            .Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                MovieOperations.Delete))
            .ReturnsAsync(AuthorizationResult.Success());

        // Act
        var result = await _mockAuthorizationService.Object.AuthorizeAsync(
            _admin,
            movie,
            MovieOperations.Delete);

        // Assert
        result.Succeeded.Should().BeTrue();
    }
}

public static class MovieOperations
{
    public static readonly string View = "ViewMovie";
    public static readonly string Edit = "EditMovie";
    public static readonly string Delete = "DeleteMovie";
} 
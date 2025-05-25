using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MovieApp.v2.Web.Controllers;
using MovieApp.v2.Web.Models;
using MovieApp.v2.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MovieApp.v2.Tests.Controllers;

[Trait("Category", "Unit")]
public class MovieControllerTests
{
    private readonly Mock<IMovieRepository> _mockRepository;
    private readonly MovieController _controller;

    public MovieControllerTests()
    {
        _mockRepository = new Mock<IMovieRepository>();
        _controller = new MovieController(_mockRepository.Object);

        // Setup default user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Name, "test@example.com"),
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithMovies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1" },
            new Movie { Id = 2, Title = "Movie 2" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(movies);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Movie>>(viewResult.Model);
        model.Should().HaveCount(2);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Movie)null);

        // Act
        var result = await _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnView_WhenMovieExists()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Test Movie" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(movie);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Movie>(viewResult.Model);
        model.Should().BeEquivalentTo(movie);
    }

    [Fact]
    public async Task Create_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "New Movie" };
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Movie>()))
            .ReturnsAsync(movie);

        // Act
        var result = await _controller.Create(movie);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        redirectResult.ActionName.Should().Be("Index");
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public async Task Create_Post_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var movie = new Movie { Id = 1 }; // Missing required Title
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Create(movie);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().BeEquivalentTo(movie);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Movie)null);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Updated Movie" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(movie);

        // Act
        var result = await _controller.Edit(1, movie);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        redirectResult.ActionName.Should().Be("Index");
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Movie)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        redirectResult.ActionName.Should().Be("Index");
        _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
} 
using Xunit;
using Moq;
using MovieApp.Web.Controllers;
using MovieApp.Web.Models;
using MovieApp.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Tests
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieRepository> _mockRepo;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _mockRepo = new Mock<IMovieRepository>();
            _controller = new MovieController(_mockRepo.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithMovieList()
        {
            // Arrange
            var movies = GetTestMovies();
            _mockRepo.Setup(repo => repo.GetAllMovies())
                .Returns(movies);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Movie>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var movie = new Movie();
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Create(movie);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(movie, viewResult.Model);
        }

        private List<Movie> GetTestMovies()
        {
            return new List<Movie>
            {
                new Movie { Id = 1, Title = "Test Movie 1", Director = "Director 1" },
                new Movie { Id = 2, Title = "Test Movie 2", Director = "Director 2" }
            };
        }
    }
} 
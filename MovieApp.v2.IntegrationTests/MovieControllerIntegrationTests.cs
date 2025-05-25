using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.v2.Web;
using MovieApp.v2.Web.Data;
using MovieApp.v2.Web.Models;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace MovieApp.v2.IntegrationTests;

[Trait("Category", "Integration")]
public class MovieControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MovieControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                // Ensure the database is created
                db.Database.EnsureCreated();

                // Seed the database with test data
                try
                {
                    SeedTestData(db);
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        });

        _client = _factory.CreateClient();
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        // Add test genres
        var genres = new List<Genre>
        {
            new Genre { Id = 1, Name = "Action", Description = "Action movies" },
            new Genre { Id = 2, Name = "Comedy", Description = "Comedy movies" }
        };
        context.Genres.AddRange(genres);

        // Add test movies
        var movies = new List<Movie>
        {
            new Movie
            {
                Title = "Test Movie 1",
                Director = "Director 1",
                Description = "Description 1",
                GenreId = 1,
                Rating = 4.5f,
                ReleaseDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.UtcNow
            },
            new Movie
            {
                Title = "Test Movie 2",
                Director = "Director 2",
                Description = "Description 2",
                GenreId = 2,
                Rating = 4.0f,
                ReleaseDate = DateTime.Now.AddYears(-2),
                CreatedAt = DateTime.UtcNow
            }
        };
        context.Movies.AddRange(movies);

        context.SaveChanges();
    }

    [Fact]
    public async Task Get_Index_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/Movie");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString()
            .Should().Contain("text/html");
    }

    [Fact]
    public async Task Get_Details_ReturnsMovie_WhenMovieExists()
    {
        // Act
        var response = await _client.GetAsync("/Movie/Details/1");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString()
            .Should().Contain("text/html");
    }

    [Fact]
    public async Task Get_Details_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/Movie/Details/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_Create_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var formData = new Dictionary<string, string>
        {
            { "Title", "New Test Movie" },
            { "Director", "New Director" },
            { "Description", "New Description" },
            { "GenreId", "1" },
            { "Rating", "4.0" },
            { "ReleaseDate", DateTime.Now.ToString("yyyy-MM-dd") }
        };

        var content = new FormUrlEncodedContent(formData);

        // Act
        var response = await _client.PostAsync("/Movie/Create", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.ToString().Should().Contain("/Movie");
    }

    [Fact]
    public async Task Post_Edit_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var formData = new Dictionary<string, string>
        {
            { "Id", "1" },
            { "Title", "Updated Movie" },
            { "Director", "Updated Director" },
            { "Description", "Updated Description" },
            { "GenreId", "1" },
            { "Rating", "4.5" },
            { "ReleaseDate", DateTime.Now.ToString("yyyy-MM-dd") }
        };

        var content = new FormUrlEncodedContent(formData);

        // Act
        var response = await _client.PostAsync("/Movie/Edit/1", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.ToString().Should().Contain("/Movie");
    }

    [Fact]
    public async Task Post_Delete_RedirectsToIndex()
    {
        // Act
        var response = await _client.PostAsync("/Movie/Delete/1", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.ToString().Should().Contain("/Movie");
    }
} 
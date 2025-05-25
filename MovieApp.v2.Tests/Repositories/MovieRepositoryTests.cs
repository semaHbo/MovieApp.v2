using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieApp.v2.Web.Data;
using MovieApp.v2.Web.Models;
using MovieApp.v2.Web.Repositories;

namespace MovieApp.v2.Tests.Repositories;

[Trait("Category", "Unit")]
public class MovieRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ApplicationDbContext _context;
    private readonly MovieRepository _repository;

    public MovieRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestMovieDb")
            .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new MovieRepository(_context);

        // Clean database before each test
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMovies()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Action" };
        await _context.Genres.AddAsync(genre);
        
        var movies = new List<Movie>
        {
            new Movie { Title = "Test Movie 1", Director = "Director 1", GenreId = 1 },
            new Movie { Title = "Test Movie 2", Director = "Director 2", GenreId = 1 }
        };
        await _context.Movies.AddRangeAsync(movies);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeInDescendingOrder(m => m.CreatedAt);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMovie_WhenMovieExists()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Action" };
        await _context.Genres.AddAsync(genre);
        
        var movie = new Movie 
        { 
            Title = "Test Movie", 
            Director = "Test Director",
            GenreId = 1 
        };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(movie.Id);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Movie");
    }

    [Fact]
    public async Task AddAsync_ShouldAddNewMovie()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Action" };
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        var movie = new Movie
        {
            Title = "New Movie",
            Director = "New Director",
            GenreId = 1
        };

        // Act
        var result = await _repository.AddAsync(movie);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMovie_WhenMovieExists()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Action" };
        await _context.Genres.AddAsync(genre);
        
        var movie = new Movie
        {
            Title = "Original Title",
            Director = "Original Director",
            GenreId = 1
        };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        movie.Title = "Updated Title";
        movie.Director = "Updated Director";

        // Act
        await _repository.UpdateAsync(movie);

        // Assert
        var updatedMovie = await _context.Movies.FindAsync(movie.Id);
        updatedMovie.Should().NotBeNull();
        updatedMovie!.Title.Should().Be("Updated Title");
        updatedMovie.Director.Should().Be("Updated Director");
        updatedMovie.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMovie_WhenMovieExists()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Action" };
        await _context.Genres.AddAsync(genre);
        
        var movie = new Movie
        {
            Title = "Test Movie",
            Director = "Test Director",
            GenreId = 1
        };
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(movie.Id);

        // Assert
        var deletedMovie = await _context.Movies.FindAsync(movie.Id);
        deletedMovie.Should().BeNull();
    }
} 
using MovieApp.v2.Web.Models;

namespace MovieApp.v2.Web.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie> AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int id);
    Task<IEnumerable<Movie>> SearchAsync(string searchTerm);
    Task<bool> ExistsAsync(int id);
} 
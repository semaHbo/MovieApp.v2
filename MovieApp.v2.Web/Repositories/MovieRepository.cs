using Microsoft.EntityFrameworkCore;
using MovieApp.v2.Web.Data;
using MovieApp.v2.Web.Models;

namespace MovieApp.v2.Web.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .Include(m => m.Genre)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie> AddAsync(Movie movie)
    {
        movie.CreatedAt = DateTime.UtcNow;
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task UpdateAsync(Movie movie)
    {
        var existingMovie = await GetByIdAsync(movie.Id);
        if (existingMovie == null)
            throw new ArgumentException("Film bulunamadı.");

        movie.UpdatedAt = DateTime.UtcNow;
        movie.CreatedAt = existingMovie.CreatedAt; // Preserve original creation date
        
        _context.Entry(existingMovie).CurrentValues.SetValues(movie);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await GetByIdAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Movie>> SearchAsync(string searchTerm)
    {
        return await _context.Movies
            .Include(m => m.Genre)
            .Where(m => m.Title.Contains(searchTerm) || 
                       m.Description.Contains(searchTerm) ||
                       m.Director.Contains(searchTerm))
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Movies.AnyAsync(m => m.Id == id);
    }
}
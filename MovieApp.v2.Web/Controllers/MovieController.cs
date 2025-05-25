using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.v2.Web.Data;
using MovieApp.v2.Web.Models;
using MovieApp.v2.Web.Repositories;

namespace MovieApp.v2.Web.Controllers;

[Authorize]
public class MovieController : Controller
{
    private readonly IMovieRepository _movieRepository;
    private readonly ApplicationDbContext _context;

    public MovieController(IMovieRepository movieRepository, ApplicationDbContext context)
    {
        _movieRepository = movieRepository;
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? searchString)
    {
        IEnumerable<Movie> movies;
        if (!string.IsNullOrEmpty(searchString))
        {
            movies = await _movieRepository.SearchAsync(searchString);
            ViewData["CurrentFilter"] = searchString;
        }
        else
        {
            movies = await _movieRepository.GetAllAsync();
        }
        return View(movies);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateGenresDropDownList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            await _movieRepository.AddAsync(movie);
            TempData["Success"] = "Film başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateGenresDropDownList(movie.GenreId);
        return View(movie);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
            return NotFound();

        await PopulateGenresDropDownList(movie.GenreId);
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Movie movie)
    {
        if (id != movie.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _movieRepository.UpdateAsync(movie);
                TempData["Success"] = "Film başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _movieRepository.ExistsAsync(movie.Id))
                    return NotFound();
                throw;
            }
        }

        await PopulateGenresDropDownList(movie.GenreId);
        return View(movie);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _movieRepository.DeleteAsync(id);
        TempData["Success"] = "Film başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateGenresDropDownList(object? selectedGenre = null)
    {
        var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        ViewBag.GenreId = new SelectList(genres, "Id", "Name", selectedGenre);
    }
} 
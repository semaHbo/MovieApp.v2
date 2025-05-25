using Microsoft.AspNetCore.Mvc;
using MovieApp.Web.Models;
using MovieApp.Web.Repositories;
using System.Collections.Generic;

namespace MovieApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public IActionResult Index()
        {
            var movies = _movieRepository.GetAllMovies();
            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = _movieRepository.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieRepository.AddMovie(movie);
                TempData["Success"] = "Film başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
    }
} 
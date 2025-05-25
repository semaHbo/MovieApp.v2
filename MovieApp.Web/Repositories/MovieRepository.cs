using MovieApp.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Web.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies;

        public MovieRepository()
        {
            // Örnek veri
            _movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    Director = "Christopher Nolan",
                    Description = "A thief who steals corporate secrets through dream-sharing technology.",
                    Point = 8.8,
                    GenreId = 1
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Shawshank Redemption",
                    Director = "Frank Darabont",
                    Description = "Two imprisoned men bond over a number of years.",
                    Point = 9.3,
                    GenreId = 2
                }
            };
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movies;
        }

        public Movie GetMovieById(int id)
        {
            return _movies.FirstOrDefault(m => m.Id == id);
        }

        public void AddMovie(Movie movie)
        {
            movie.Id = _movies.Max(m => m.Id) + 1;
            _movies.Add(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            var existingMovie = GetMovieById(movie.Id);
            if (existingMovie != null)
            {
                var index = _movies.IndexOf(existingMovie);
                _movies[index] = movie;
            }
        }

        public void DeleteMovie(int id)
        {
            var movie = GetMovieById(id);
            if (movie != null)
            {
                _movies.Remove(movie);
            }
        }
    }
} 
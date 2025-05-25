using MovieApp.Web.Models;
using System.Collections.Generic;

namespace MovieApp.Web.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(int id);
    }
}
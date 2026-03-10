using Filmy.Models;

namespace Filmy.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> SearchMoviesAsync(string query);
        Task<Movie> GetMovieDetailsAsync(int movieId);
    }
}

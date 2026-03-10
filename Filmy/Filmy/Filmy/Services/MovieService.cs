using Filmy.Models;
using Newtonsoft.Json;

namespace Filmy.Services
{
    public class MovieService : IMovieService
    {
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ApiKey = "b6d94db57660cc6c91e80206bd37a63f";
        private readonly HttpClient _httpClient;

        public MovieService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            try
            {
                var url = $"{BaseUrl}/search/movie?api_key={ApiKey}&query={Uri.EscapeDataString(query)}&language=en-US";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<Movie>();

                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(content);

                if (movieResponse?.Results == null)
                    return new List<Movie>();

                return movieResponse.Results.Select(dto => new Movie
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Overview = dto.Overview,
                    VoteAverage = dto.VoteAverage,
                    ReleaseDate = dto.ReleaseDate,
                    PosterPath = dto.PosterPath,
                    BackdropPath = dto.BackdropPath,
                    UserRating = 0,
                    IsFavorite = false
                }).ToList();
            }
            catch (Exception)
            {
                return new List<Movie>();
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            try
            {
                var url = $"{BaseUrl}/movie/{movieId}?api_key={ApiKey}&language=en-US";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var movieDto = JsonConvert.DeserializeObject<MovieDto>(content);

                if (movieDto == null)
                    return null;

                return new Movie
                {
                    Id = movieDto.Id,
                    Title = movieDto.Title,
                    Overview = movieDto.Overview,
                    VoteAverage = movieDto.VoteAverage,
                    ReleaseDate = movieDto.ReleaseDate,
                    PosterPath = movieDto.PosterPath,
                    BackdropPath = movieDto.BackdropPath,
                    UserRating = 0,
                    IsFavorite = false
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

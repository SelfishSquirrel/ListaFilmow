using Filmy.Models;
using Newtonsoft.Json;

namespace Filmy.Services
{
    public class FavoritesService : IFavoritesService
    {
        private const string FavoritesFileName = "favorites.json";
        private readonly string _documentsPath;

        public FavoritesService()
        {
            _documentsPath = Path.Combine(FileSystem.AppDataDirectory);
        }

        public async Task SaveFavoritesAsync(List<Movie> movies)
        {
            try
            {
                var filePath = Path.Combine(_documentsPath, FavoritesFileName);
                var json = JsonConvert.SerializeObject(movies);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception)
            {
            }
        }

        public async Task<List<Movie>> LoadFavoritesAsync()
        {
            try
            {
                var filePath = Path.Combine(_documentsPath, FavoritesFileName);

                if (!File.Exists(filePath))
                    return new List<Movie>();

                var json = await File.ReadAllTextAsync(filePath);
                var favorites = JsonConvert.DeserializeObject<List<Movie>>(json);
                return favorites ?? new List<Movie>();
            }
            catch (Exception)
            {
                return new List<Movie>();
            }
        }
    }
}

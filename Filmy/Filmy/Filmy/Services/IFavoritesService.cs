using Filmy.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Filmy.Services
{
    public interface IFavoritesService
    {
        Task SaveFavoritesAsync(List<Movie> movies);
        Task<List<Movie>> LoadFavoritesAsync();
    }
}

using Filmy.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Filmy.ViewModels
{
    public class MovieSearchViewModel : INotifyPropertyChanged
    {
        private string _searchQuery;
        private bool _isSearching;
        private Movie _selectedMovie;
        private readonly Services.IMovieService _movieService;
        private readonly Services.IFavoritesService _favoritesService;

        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Movie> _favorites;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                if (_isSearching != value)
                {
                    _isSearching = value;
                    OnPropertyChanged();
                }
            }
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                if (_selectedMovie != value)
                {
                    _selectedMovie = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                if (_movies != value)
                {
                    _movies = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Movie> Favorites
        {
            get => _favorites;
            set
            {
                if (_favorites != value)
                {
                    _favorites = value;
                    OnPropertyChanged();
                }
            }
        }

        public MovieSearchViewModel()
        {
            _movieService = new Services.MovieService();
            _favoritesService = new Services.FavoritesService();
            Movies = new ObservableCollection<Movie>();
            Favorites = new ObservableCollection<Movie>();
            SearchQuery = "";
            LoadFavorites();
        }

        public async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                Movies.Clear();
                return;
            }

            IsSearching = true;
            try
            {
                var results = await _movieService.SearchMoviesAsync(SearchQuery);
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Movies.Clear();
                    foreach (var movie in results)
                    {
                        var favorite = Favorites.FirstOrDefault(f => f.Id == movie.Id);
                        if (favorite != null)
                        {
                            movie.IsFavorite = favorite.IsFavorite;
                            movie.UserRating = favorite.UserRating;
                        }
                        Movies.Add(movie);
                    }
                });
            }
            finally
            {
                IsSearching = false;
            }
        }

        public async Task AddToFavoritesAsync(Movie movie)
        {
            if (movie == null) return;

            var existing = Favorites.FirstOrDefault(f => f.Id == movie.Id);
            if (existing == null)
            {
                movie.IsFavorite = true;
                Favorites.Add(movie);
                
                var movieInSearch = Movies.FirstOrDefault(m => m.Id == movie.Id);
                if (movieInSearch != null)
                {
                    movieInSearch.IsFavorite = true;
                }

                await _favoritesService.SaveFavoritesAsync(Favorites.ToList());
            }
        }

        public async Task RemoveFromFavoritesAsync(Movie movie)
        {
            if (movie == null) return;

            var existing = Favorites.FirstOrDefault(f => f.Id == movie.Id);
            if (existing != null)
            {
                Favorites.Remove(existing);
                
                var movieInSearch = Movies.FirstOrDefault(m => m.Id == movie.Id);
                if (movieInSearch != null)
                {
                    movieInSearch.IsFavorite = false;
                }

                await _favoritesService.SaveFavoritesAsync(Favorites.ToList());
            }
        }

        public async Task RateMovieAsync(Movie movie, double rating)
        {
            if (movie == null) return;

            var existing = Favorites.FirstOrDefault(f => f.Id == movie.Id);
            if (existing != null)
            {
                existing.UserRating = rating;
                
                var movieInSearch = Movies.FirstOrDefault(m => m.Id == movie.Id);
                if (movieInSearch != null)
                {
                    movieInSearch.UserRating = rating;
                }

                await _favoritesService.SaveFavoritesAsync(Favorites.ToList());
            }
        }

        private async void LoadFavorites()
        {
            var favorites = await _favoritesService.LoadFavoritesAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Favorites.Clear();
                foreach (var fav in favorites)
                {
                    Favorites.Add(fav);
                }
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

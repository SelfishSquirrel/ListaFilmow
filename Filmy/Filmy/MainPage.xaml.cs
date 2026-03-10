using Filmy.ViewModels;

namespace Filmy
{
    public partial class MainPage : ContentPage
    {
        private MovieSearchViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MovieSearchViewModel();
            BindingContext = _viewModel;
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            await _viewModel.SearchAsync();
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            await _viewModel.SearchAsync();
        }

        private void OnViewDetails(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Models.Movie movie)
            {
                Navigation.PushAsync(new MovieDetailsPage(movie, _viewModel));
            }
        }

        private async void OnToggleFavorite(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Models.Movie movie)
            {
                if (movie.IsFavorite)
                {
                    await _viewModel.RemoveFromFavoritesAsync(movie);
                }
                else
                {
                    await _viewModel.AddToFavoritesAsync(movie);
                }
            }
        }

        private void ShowSearchResults(object sender, EventArgs e)
        {
            MoviesCollectionView.ItemsSource = _viewModel.Movies;
        }

        private void ShowFavorites(object sender, EventArgs e)
        {
            MoviesCollectionView.ItemsSource = _viewModel.Favorites;
        }
    }
}

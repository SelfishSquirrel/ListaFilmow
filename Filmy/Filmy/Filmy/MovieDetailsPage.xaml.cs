using Filmy.Models;
using Filmy.ViewModels;

namespace Filmy
{
    public partial class MovieDetailsPage : ContentPage
    {
        private Movie _movie;
        private MovieSearchViewModel _viewModel;
        private double[] _ratings = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public MovieDetailsPage(Movie movie, MovieSearchViewModel viewModel)
        {
            InitializeComponent();
            _movie = movie;
            _viewModel = viewModel;
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadMovieDetails();
            });
        }

        private void LoadMovieDetails()
        {
            if (TitleLabel != null)
                TitleLabel.Text = _movie.Title;
            
            if (OverviewLabel != null)
                OverviewLabel.Text = _movie.Overview;
            
            if (ReleaseDateLabel != null)
                ReleaseDateLabel.Text = _movie.ReleaseDate;
            
            if (RatingLabel != null)
                RatingLabel.Text = $"{_movie.VoteAverage:F1}/10";

            if (PosterImage != null && !string.IsNullOrEmpty(_movie.PosterPath))
            {
                PosterImage.Source = new UriImageSource
                {
                    Uri = new Uri($"https://image.tmdb.org/t/p/w500{_movie.PosterPath}")
                };
            }

            if (UserRatingPicker != null)
            {
                UserRatingPicker.ItemsSource = _ratings.Select(r => r.ToString()).ToList();
                UserRatingPicker.SelectedIndex = (int)_movie.UserRating;
            }

            UpdateFavoriteButton();
        }

        private void UpdateFavoriteButton()
        {
            if (FavoriteButton != null)
            {
                FavoriteButton.Text = _movie.IsFavorite ? "Remove from Favorites" : "Add to Favorites";
                FavoriteButton.BackgroundColor = _movie.IsFavorite ? Color.FromArgb("#ff6b6b") : Color.FromArgb("#51cf66");
            }
        }

        private async void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (_movie.IsFavorite)
            {
                await _viewModel.RemoveFromFavoritesAsync(_movie);
            }
            else
            {
                await _viewModel.AddToFavoritesAsync(_movie);
            }
            UpdateFavoriteButton();
        }

        private async void OnRatingChanged(object sender, EventArgs e)
        {
            if (UserRatingPicker != null && UserRatingPicker.SelectedIndex >= 0)
            {
                _movie.UserRating = _ratings[UserRatingPicker.SelectedIndex];
                
                if (_movie.IsFavorite)
                {
                    await _viewModel.RateMovieAsync(_movie, _movie.UserRating);
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

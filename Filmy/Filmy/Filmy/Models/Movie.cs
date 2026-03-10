namespace Filmy.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public double VoteAverage { get; set; }
        public string ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
        public double UserRating { get; set; }
        public bool IsFavorite { get; set; }
    }
}

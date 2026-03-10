namespace Filmy.Models
{
    public class MovieResponse
    {
        [Newtonsoft.Json.JsonProperty("results")]
        public List<MovieDto> Results { get; set; } = new();
    }

    public class MovieDto
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("title")]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("overview")]
        public string Overview { get; set; }

        [Newtonsoft.Json.JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [Newtonsoft.Json.JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [Newtonsoft.Json.JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [Newtonsoft.Json.JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
    }
}

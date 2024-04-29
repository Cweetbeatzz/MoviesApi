namespace MoviesApi.Model
{
    public class IMDBModel
    {
        public class Movie
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Poster { get; set; } 
            public string Plot { get; set; }
            public string ImdbRating { get; set; }
        }

        public class SearchResponse
        {
            public List<Movie> Search { get; set; } 
        }
    }
}

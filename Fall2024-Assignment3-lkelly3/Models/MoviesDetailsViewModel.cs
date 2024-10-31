namespace Fall2024_Assignment3_lkelly3.Models
{
    public class MoviesDetailsViewModel
    {
        public Movies Movie { get; set; }
        public IEnumerable<Actors> Actors { get; set; }
        public List<Review> Reviews { get; set; }
        public string Sentiment { get; set; }

        public MoviesDetailsViewModel(Movies movie, IEnumerable<Actors> actors, List<Review> reviews, string sentiment)
        {
            Movie = movie;
            Actors = actors;
            Reviews = reviews;
            Sentiment = sentiment;
        }
    }
}

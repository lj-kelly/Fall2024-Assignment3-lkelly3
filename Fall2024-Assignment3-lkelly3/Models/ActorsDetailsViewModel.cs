namespace Fall2024_Assignment3_lkelly3.Models;

public class ActorsDetailsViewModel
{
    public Actors Actor { get; set; }
    public IEnumerable<Movies> Movies { get; set; }
    public List<Review> Tweets { get; set; }
    public string? Sentiment { get; set; }

    public ActorsDetailsViewModel(Actors actor, IEnumerable<Movies> movies, List<Review> tweets, string sentiment)
    {
        Actor = actor;
        Movies = movies;
        Tweets = tweets;
        Sentiment = sentiment;

    }
}
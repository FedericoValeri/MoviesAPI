namespace MoviesAPI.Models.Entities
{
    public class MovieMovieTheater
    {
        public MovieMovieTheater(int movieId, int movieTheaterId)
        {
            MovieId = movieId;
            MovieTheaterId = movieTheaterId;
        }

        public int MovieId { get; set; }
        public int MovieTheaterId { get; set; }
        public Movie Movie { get; set; }
        public MovieTheater MovieTheater { get; set; }
    }
}
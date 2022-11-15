using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Entities
{
    public class MovieActor
    {
        public MovieActor(int movieId, int actorId)
        {
            MovieId = movieId;
            ActorId = actorId;
        }

        public int MovieId { get; set; }
        public int ActorId { get; set; }

        [StringLength(75)]
        public string Character { get; set; }

        public int Order { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
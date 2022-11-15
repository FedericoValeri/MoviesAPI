using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string Title { get; set; }

        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<MovieTheater> MovieTheaters { get; set; }
        public ICollection<Actor> Actors { get; set; }
    }
}
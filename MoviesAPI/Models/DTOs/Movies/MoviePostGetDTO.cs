using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.DTOs.MovieTheaters;

namespace MoviesAPI.Models.DTOs.Movies
{
    public class MoviePostGetDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<MovieTheaterDTO> MovieTheaters { get; set; }
    }
}
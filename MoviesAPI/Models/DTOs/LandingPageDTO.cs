using MoviesAPI.Models.DTOs.Movies;

namespace MoviesAPI.Models.DTOs
{
    public class LandingPageDTO
    {
        public List<MovieDTO> InTheaters { get; set; }
        public List<MovieDTO> UpcomingReleases { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.MovieTheaters
{
    public class MovieTheaterCreateDTO
    {
        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
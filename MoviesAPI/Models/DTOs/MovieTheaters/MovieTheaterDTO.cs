using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.MovieTheaters
{
    public class MovieTheaterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
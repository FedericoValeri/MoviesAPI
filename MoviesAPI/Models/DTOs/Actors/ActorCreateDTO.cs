using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.Actors
{
    public class ActorCreateDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }

        public IFormFile Picture { get; set; }
    }
}
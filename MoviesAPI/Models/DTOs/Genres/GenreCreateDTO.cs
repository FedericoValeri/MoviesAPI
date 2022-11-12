using MoviesAPI.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.Genres
{
    public class GenreCreateDTO
    {
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
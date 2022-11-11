using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field with name {0} is required")]
        public string Name { get; set; }
    }
}
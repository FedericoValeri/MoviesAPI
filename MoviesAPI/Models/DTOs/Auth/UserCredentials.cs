using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.Auth
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
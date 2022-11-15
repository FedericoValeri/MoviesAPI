﻿using MoviesAPI.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
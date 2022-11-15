﻿using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Entities
{
    public class MovieTheater
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 75)]
        public string Name { get; set; }

        public Point Location { get; set; }
    }
}
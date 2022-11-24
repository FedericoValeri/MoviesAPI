﻿using MoviesAPI.Models.DTOs.Actors;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.DTOs.MovieTheaters;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.DTOs.Movies
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public string? Summary { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Poster { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public List<MovieTheaterDTO> MovieTheaters { get; set; }
        public List<ActorsMovieDTO> Actors { get; set; }
    }
}
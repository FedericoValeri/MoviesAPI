using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Customizations.ModelBinders;
using MoviesAPI.Models.DTOs.Actors;

namespace MoviesAPI.Models.DTOs.Movies
{
    public class MovieCreateDTO
    {
        public string Title { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public string? Summary { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile? Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int>? GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int>? MovieTheatersIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<MovieActorCreateDTO>>))]
        public List<MovieActorCreateDTO>? Actors { get; set; }
    }
}
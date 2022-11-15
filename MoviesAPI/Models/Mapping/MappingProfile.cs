using AutoMapper;
using Microsoft.Extensions.Options;
using MoviesAPI.Models.DTOs.Actors;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.DTOs.Movies;
using MoviesAPI.Models.DTOs.MovieTheaters;
using MoviesAPI.Models.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(GeometryFactory geometryFactory)
        {
            // Genre
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreateDTO, Genre>();

            // Actor
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>()
                .ForMember(a => a.Picture, options => options.Ignore());

            // MovieTheater
            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(dto => dto.Latitude, options => options.MapFrom(m => m.Location.Y))
                .ForMember(dto => dto.Longitude, options => options.MapFrom(m => m.Location.X));
            CreateMap<MovieTheaterCreateDTO, MovieTheater>()
                .ForMember(m => m.Location, options => options.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            // Movie
            CreateMap<MovieCreateDTO, Movie>()
                .ForMember(m => m.Poster, options => options.Ignore())
                .ForMember(m => m.Genres, options => options.MapFrom(MapGenres))
                .ForMember(m => m.MovieTheaters, options => options.MapFrom(MapMovieTheaters))
                .ForMember(m => m.Actors, options => options.MapFrom(MapActors));
        }

        private static List<Genre> MapGenres(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            List<Genre> result = new();

            if (movieCreateDTO.GenresIds != null)
            {
                foreach (var id in movieCreateDTO.GenresIds)
                {
                    result.Add(new Genre() { Id = id });
                }
            }

            return result;
        }

        private static List<MovieTheater> MapMovieTheaters(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            List<MovieTheater> result = new();

            if (movieCreateDTO.MovieTheatersIds != null)
            {
                foreach (var id in movieCreateDTO.MovieTheatersIds)
                {
                    result.Add(new MovieTheater() { Id = id });
                }
            }

            return result;
        }

        private static List<MovieActor> MapActors(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            List<MovieActor> result = new();

            if (movieCreateDTO.Actors != null)
            {
                foreach (var actor in movieCreateDTO.Actors)
                {
                    result.Add(new MovieActor(movie.Id, actor.Id)
                    {
                        Character = actor.Character,
                    });
                }
            }

            return result;
        }
    }
}
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
                .ForMember(m => m.Genres, options => options.Ignore())
                .ForMember(m => m.MovieTheaters, options => options.Ignore())
                .ForMember(m => m.Actors, options => options.Ignore());
        }
    }
}
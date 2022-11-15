using AutoMapper;
using MoviesAPI.Models.DTOs.Actors;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.DTOs.MovieTheaters;
using MoviesAPI.Models.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(GeometryFactory geometryFactory)
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreateDTO, Genre>();

            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>().ForMember(a => a.Picture, options => options.Ignore());

            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(dto => dto.Latitude, config => config.MapFrom(m => m.Location.Y))
                .ForMember(dto => dto.Longitude, config => config.MapFrom(m => m.Location.X));
            CreateMap<MovieTheaterCreateDTO, MovieTheater>()
                .ForMember(m => m.Location, config => config.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));
        }
    }
}
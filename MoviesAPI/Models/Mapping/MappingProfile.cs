using AutoMapper;
using MoviesAPI.Models.DTOs.Actors;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.Entities;

namespace MoviesAPI.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreateDTO, Genre>();
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>().ForMember(a => a.Picture, options => options.Ignore());
        }
    }
}
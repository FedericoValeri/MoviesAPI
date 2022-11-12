using AutoMapper;
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
        }
    }
}
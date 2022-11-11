using MoviesAPI.Models.Entities;

namespace MoviesAPI.Models.Services
{
    public interface IRepository
    {
        List<Genre> GetAllGenres();
    }
}
using MoviesAPI.Models.Entities;

namespace MoviesAPI.Models.Services
{
    public interface IRepository
    {
        Task<List<Genre>> GetAllGenresAsync();

        Genre GetGenre(int id);
    }
}
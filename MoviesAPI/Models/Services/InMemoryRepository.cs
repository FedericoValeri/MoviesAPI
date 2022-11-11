using MoviesAPI.Models.Entities;

namespace MoviesAPI.Models.Services
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Genre> _genres;

        public InMemoryRepository()
        {
            _genres = new List<Genre>()
            {
                new Genre()
                {
                    Id = 1,
                    Name = "Comedy"
                },
                new Genre()
                {
                    Id = 2,
                    Name = "Action"
                }
            };
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            await Task.Delay(1);
            return _genres;
        }

        public Genre GetGenre(int id)
        {
            return _genres.Find(g => g.Id == id);
        }
    }
}
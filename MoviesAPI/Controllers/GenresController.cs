using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepository repository;

        public GenresController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await repository.GetAllGenresAsync());
        }

        // GET: api/genres/{id}
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Genre genre = repository.GetGenre(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Genre genre)
        {
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Genre genre)
        {
            return NoContent();
        }

        [HttpDelete]
        public void Delete()
        {
        }
    }
}
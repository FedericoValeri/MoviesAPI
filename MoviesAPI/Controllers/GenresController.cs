using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;

        public GenresController(ILogger<GenresController> logger)
        {
            this.logger = logger;
        }

        // GET: api/genres
        [HttpGet]
        public IActionResult GetAll()
        {
            logger.LogInformation("Getting all the genres");
            return Ok(new List<Genre>() { new Genre() { Id = 1, Name = "Comedy" } });
        }

        // GET: api/genres/{id}
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
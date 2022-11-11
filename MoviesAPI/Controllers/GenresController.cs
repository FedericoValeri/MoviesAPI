using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<GenresController> logger;

        public GenresController(
            ApplicationDbContext context,
            ILogger<GenresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Getting all the genres");
            return Ok(await context.Genres.ToListAsync());
        }

        // GET: api/genres/{id}
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genre genre)
        {
            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
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
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Customizations.Helpers;
using MoviesAPI.Models.DTOs;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GenresController> logger;

        public GenresController(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<GenresController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/genres/
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTO paginationDTO)
        {
            IQueryable<Genre> query = context.Genres.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(query);
            List<Genre> genres = await query
                .OrderBy(g => g.Name)
                .Paginate(paginationDTO)
                .ToListAsync();
            return Ok(mapper.Map<List<GenreDTO>>(genres));
        }

        // GET: api/genres/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            Genre genre = await context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<GenreDTO>(genre));
        }

        // POST: api/genres/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreCreateDTO genreDTO)
        {
            Genre genre = mapper.Map<Genre>(genreDTO);
            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] GenreCreateDTO genreCreateDTO)
        {
            Genre genre = await context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            mapper.Map(genreCreateDTO, genre);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool exists = await context.Genres.AnyAsync(g => g.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Genre() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
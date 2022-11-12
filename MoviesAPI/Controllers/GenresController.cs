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
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
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
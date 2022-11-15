using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models.DTOs;
using MoviesAPI.Models.DTOs.Genres;
using MoviesAPI.Models.DTOs.Movies;
using MoviesAPI.Models.DTOs.MovieTheaters;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(
            ApplicationDbContext context,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        // GET: api/Movies
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Movie>>> GetAll(PaginationDTO paginationDTO)
        //{
        //    return await context.Movies.ToListAsync();
        //}

        // GET: api/Movies/{id}
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Movie>> Get(int id)
        //{
        //    var movie = await context.Movies.FindAsync(id);

        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return movie;
        //}

        // PUT: api/Movies/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> Put(int id, Movie movie)
        //{
        //    return NoContent();
        //}

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var genres = await context.Genres.ToListAsync();
            var movieTheaters = await context.MovieTheaters.ToListAsync();
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            var movieTheatersDTO = mapper.Map<List<MovieTheaterDTO>>(movieTheaters);

            MoviePostGetDTO moviePostGetDTO = new()
            {
                Genres = genresDTO,
                MovieTheaters = movieTheatersDTO
            };

            return moviePostGetDTO;
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> Post([FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = mapper.Map<Movie>(movieCreateDTO);

            if (movieCreateDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(containerName, movieCreateDTO.Poster);
            }

            AnnotateActorsOrder(movie);
            context.Movies.Add(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Movies/{id}
        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var movie = await context.Movies.FindAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    context.Movies.Remove(movie);
        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}

        private void AnnotateActorsOrder(Movie movie)
        {
            if (movie != null)
            {
                IEnumerable<MovieActor> movieActors = context.MovieActors
                    .Where(ma => ma.MovieId == movie.Id)
                    .ToList();

                if (movieActors.Any())
                {
                    for (int i = 0; i < movieActors.Count(); i++)
                    {
                        movieActors.ElementAt(i).Order = i;
                    }
                }
            }
        }
    }
}
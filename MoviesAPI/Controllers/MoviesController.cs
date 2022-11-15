using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<int>> Post([FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = mapper.Map<Movie>(movieCreateDTO);

            if (movieCreateDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(containerName, movieCreateDTO.Poster);
            }

            context.Add(movie);
            await context.SaveChangesAsync();

            if (movieCreateDTO.GenresIds != null)
            {
                foreach (var id in movieCreateDTO.GenresIds)
                {
                    MovieGenre movieGenre = new(movie.Id, id);
                    context.MovieGenres.Add(movieGenre);
                }
            }

            if (movieCreateDTO.MovieTheatersIds != null)
            {
                foreach (var id in movieCreateDTO.MovieTheatersIds)
                {
                    MovieMovieTheater movieMovieTheater = new(movie.Id, id);
                    context.MovieMovieTheaters.Add(movieMovieTheater);
                }
            }

            if (movieCreateDTO.Actors != null)
            {
                for (int i = 0; i < movieCreateDTO.Actors.Count; i++)
                {
                    MovieActor movieActor = new(movie.Id, movieCreateDTO.Actors.ElementAt(i).Id)
                    {
                        Character = movieCreateDTO.Actors.ElementAt(i).Character,
                        Order = i
                    };

                    context.MovieActors.Add(movieActor);
                }
            }

            await context.SaveChangesAsync();

            return movie.Id;
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
    }
}
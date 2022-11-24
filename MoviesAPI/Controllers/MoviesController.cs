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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LandingPageDTO>>> Get()
        {
            int top = 6;
            DateTime today = DateTime.Today;

            var upcomingReleases = await context.Movies
                .Where(m => m.ReleaseDate > today)
                .OrderBy(m => m.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var inTheaters = await context.Movies
                .Where(m => m.InTheaters)
                .OrderBy(m => m.ReleaseDate)
                .Take(top)
                .ToListAsync();

            LandingPageDTO dto = new()
            {
                UpcomingReleases = mapper.Map<List<MovieDTO>>(upcomingReleases),
                InTheaters = mapper.Map<List<MovieDTO>>(inTheaters)
            };

            return Ok(dto);
        }

        // GET: api/Movies/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(m => m.Genres)
                .Include(m => m.MovieTheaters)
                .Include(m => m.Actors)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<MovieDTO>(movie);
            // Character, Picture and Order are missing in mapping so we do mapping here
            foreach (var actorMovie in dto.Actors)
            {
                var movieActorInDb = context.MovieActors.SingleOrDefault(ma => ma.MovieId == id && ma.ActorId == actorMovie.Id);
                if (movieActorInDb != null)
                {
                    actorMovie.Character = movieActorInDb.Character;
                    actorMovie.Order = movieActorInDb.Order;
                }

                var actorInDb = context.Actors.Find(actorMovie.Id);
                if (actorInDb != null)
                {
                    actorMovie.Picture = actorInDb.Picture;
                }
            }

            dto.Actors = dto.Actors.OrderBy(a => a.Order).ToList();
            return dto;
        }

        [HttpGet("putget/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            var movie = movieActionResult.Value;
            var genreSelectedIds = movie.Genres.Select(g => g.Id).ToList();
            var nonSelectedGenres = await context.Genres
                .Where(g => !genreSelectedIds.Contains(g.Id))
                .ToListAsync();
            var movieTheatersIds = movie.MovieTheaters.Select(m => m.Id).ToList();
            var nonSelectedMovieTheaters = await context.MovieTheaters
                .Where(m => !movieTheatersIds.Contains(m.Id))
                .ToListAsync();
            var nonSelectedGenresDTOs = mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMovieTheatersDTOs = mapper.Map<List<MovieTheaterDTO>>(nonSelectedMovieTheaters);

            MoviePutGetDTO response = new()
            {
                Movie = movie,
                SelectedGenres = movie.Genres,
                NonSelectedGenres = nonSelectedGenresDTOs,
                SelectedMovieTheaters = movie.MovieTheaters,
                NonSelectedMovieTheaters = nonSelectedMovieTheatersDTOs,
                Actors = movie.Actors
            };

            return response;
        }

        // PUT: api/Movies/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = await context.Movies
                .Include(m => m.Actors)
                .Include(m => m.MovieTheaters)
                .Include(m => m.Genres)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            mapper.Map(movieCreateDTO, movie);

            if (movieCreateDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.EditFile(containerName, movieCreateDTO.Poster, movie.Poster);
            }

            //TODO: update all relationships
            if (movieCreateDTO.GenresIds != null)
            {
                var movieGenres = context.MovieGenres.Where(mg => mg.MovieId == movie.Id).ToList();
                context.MovieGenres.RemoveRange(movieGenres);

                foreach (var genreId in movieCreateDTO.GenresIds)
                {
                    MovieGenre movieGenre = new(movie.Id, genreId);
                    context.MovieGenres.Add(movieGenre);
                }
            }

            if (movieCreateDTO.MovieTheatersIds != null)
            {
                var movieMovieTheaters = context.MovieMovieTheaters.Where(mg => mg.MovieId == movie.Id).ToList();
                context.MovieMovieTheaters.RemoveRange(movieMovieTheaters);

                foreach (var movieTheaterId in movieCreateDTO.MovieTheatersIds)
                {
                    MovieMovieTheater movieMovieTheater = new(movie.Id, movieTheaterId);
                    context.MovieMovieTheaters.Add(movieMovieTheater);
                }
            }

            if (movieCreateDTO.Actors != null)
            {
                var movieActors = context.MovieActors.Where(mg => mg.MovieId == movie.Id).ToList();
                context.MovieActors.RemoveRange(movieActors);

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
            return NoContent();
        }

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
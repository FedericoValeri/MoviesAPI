﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Customizations.Helpers;
using MoviesAPI.Models.DTOs;
using MoviesAPI.Models.DTOs.MovieTheaters;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MovieTheatersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MovieTheatersController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/MovieTheaters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieTheaterDTO>>> GetAll([FromQuery] PaginationDTO paginationDTO)
        {
            var query = context.MovieTheaters.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(query);
            var movieTheaters = await context.MovieTheaters
                .OrderBy(m => m.Name)
                .Paginate(paginationDTO)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<MovieTheaterDTO>>(movieTheaters));
        }

        // GET: api/MovieTheaters/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
        {
            var movieTheater = await context.MovieTheaters.FindAsync(id);

            if (movieTheater == null)
            {
                return NotFound();
            }

            return mapper.Map<MovieTheaterDTO>(movieTheater);
        }

        // PUT: api/MovieTheaters/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, MovieTheaterCreateDTO movieTheaterCreateDTO)
        {
            var movieTheater = await context.MovieTheaters.FindAsync(id);

            if (movieTheater == null)
            {
                return NotFound();
            }

            mapper.Map(movieTheaterCreateDTO, movieTheater);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/MovieTheaters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieTheater>> Post(MovieTheaterCreateDTO movieTheaterCreateDTO)
        {
            var movieTheater = mapper.Map<MovieTheater>(movieTheaterCreateDTO);
            context.MovieTheaters.Add(movieTheater);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/MovieTheaters/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieTheater = await context.MovieTheaters.FindAsync(id);

            if (movieTheater == null)
            {
                return NotFound();
            }

            context.MovieTheaters.Remove(movieTheater);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models.DTOs.Actors;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "actors";

        public ActorsController(
            ApplicationDbContext context,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetAll()
        {
            IEnumerable<Actor> actors = await context.Actors.ToListAsync();
            return Ok(mapper.Map<IEnumerable<ActorDTO>>(actors));
        }

        // GET: api/Actors/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            var actor = await context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ActorDTO>(actor));
        }

        // PUT: api/Actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] ActorCreateDTO actorCreateDTO)
        {
            //if (id != actor.Id)
            //{
            //    return BadRequest();
            //}

            //context.Entry(actor).State = EntityState.Modified;

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ActorExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            throw new NotImplementedException();
        }

        // POST: api/Actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> Post([FromForm] ActorCreateDTO actorCreateDTO)
        {
            var actor = mapper.Map<Actor>(actorCreateDTO);

            if (actorCreateDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.SaveFile(containerName, actorCreateDTO.Picture);
            }

            context.Actors.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            context.Actors.Remove(actor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return context.Actors.Any(e => e.Id == id);
        }
    }
}
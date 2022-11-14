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
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public ActorsController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetActors()
        {
            IEnumerable<Actor> actors = await _context.Actors.ToListAsync();
            return Ok(mapper.Map<IEnumerable<ActorDTO>>(actors));
        }

        // GET: api/Actors/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ActorDTO>(actor));
        }

        // PUT: api/Actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, ActorCreateDTO actorCreateDTO)
        {
            //if (id != actor.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(actor).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
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
        public async Task<ActionResult<Actor>> PostActor(ActorCreateDTO actorCreateDTO)
        {
            //_context.Actors.Add(actor);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
            throw new NotImplementedException();
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
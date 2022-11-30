using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models.DTOs.Ratings;
using MoviesAPI.Models.Entities;
using MoviesAPI.Models.Services.Infrastructure;
using System.Security.Claims;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public RatingsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;
            var currentRate = await context.Ratings.SingleOrDefaultAsync(r => r.MovieId == ratingDTO.MovieId && r.UserId == userId);
            if (currentRate == null)
            {
                Rating rating = new()
                {
                    MovieId = ratingDTO.MovieId,
                    Rate = ratingDTO.Rating,
                    UserId = userId
                };
                context.Ratings.Add(rating);
            }
            else
            {
                currentRate.Rate = ratingDTO.Rating;
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
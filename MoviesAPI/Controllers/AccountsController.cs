using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Customizations.Helpers;
using MoviesAPI.Models.DTOs;
using MoviesAPI.Models.DTOs.Auth;
using MoviesAPI.Models.Services.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AccountsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] UserCredentials userCredentials)
        {
            IdentityUser user = new()
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email
            };

            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return await BuildTokenAsync(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await BuildTokenAsync(userCredentials);
            }
            else
            {
                return BadRequest("Incorrect login");
            }
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult<List<UserDTO>>> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var query = context.Users.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(query);
            var users = await query
                .OrderBy(user => user.Email)
                .Paginate(paginationDTO)
                .ToListAsync();

            return mapper.Map<List<UserDTO>>(users);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> MakeAdmin([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> RemoveAdmin([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }

        private async Task<AuthenticationResponse> BuildTokenAsync(UserCredentials userCredentials)
        {
            List<Claim> claims = new()
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddYears(1);
            JwtSecurityToken token = new(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.Services;

namespace MoviesAPI.Controllers
{
    public class GenresController : Controller
    {
        private readonly IRepository repository;

        public GenresController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
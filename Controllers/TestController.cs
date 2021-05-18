using Microsoft.AspNetCore.Mvc;
using MovieMvc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public TestController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_movieService.FindById(id));
        }

    }
}

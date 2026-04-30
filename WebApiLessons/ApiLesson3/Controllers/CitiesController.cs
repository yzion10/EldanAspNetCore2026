using ApiLesson3.DataStores;
using ApiLesson3.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson3.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CityDTO> GetCities()
        {
            _logger.LogInformation("Getting all cities");
            return CitiesDataStore.Current;
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            _logger.LogInformation($"Getting city with ID {id}");

            var city = CitiesDataStore.
                Current.
                FirstOrDefault(c => c.ID == id);

            if (city == null)
            {
                _logger.LogWarning($"City with ID {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"Returning city with ID {id}");

            return city;
        }

        [HttpGet("GetCitiesHardCoded")]
        public List<CityDTO> GetCities1()
        {
            return new List<CityDTO>()
            {
                new CityDTO{ ID = 1, Name = "Tel-Aviv" },
                new CityDTO{ ID = 2, Name = "Jerusalem" },
            };
        }

        [HttpGet("problem")]
        public ActionResult MakeAProblem()
        {
            var problem = new ProblemDetails
            {
                Status = 500,
                Title = "Something went wrong",
                Detail = "An unexpected error occurred while processing your request."
            };

            return StatusCode(500, problem);
        }
    }
}

using ApiLesson3.DataStores;
using ApiLesson3.DTO;
using ApiLesson3.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace ApiLesson3.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IEmailService _email;

        public CitiesController(ILogger<CitiesController> logger, IEmailService email)
        {
            _logger = logger;
            _email = email;
        }

        [HttpGet]
        public IEnumerable<CityDTO> GetCities()
        {
            _logger.LogInformation("No Property here");

            using (LogContext.PushProperty("SessionID", Guid.NewGuid()))
            {
                //_logger.LogInformation("Getting all cities");
                _logger.LogInformation("Getting all cities from data store");
                _logger.LogInformation($"Number of cities in data store: {CitiesDataStore.Current.Count}");

                _email.Send("Cities Retrieved", $"All cities were retrieved. Total count: {CitiesDataStore.Current.Count}");

                return CitiesDataStore.Current;
            }
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

            _email.Send("City Retrieved", $"City with ID {id} was retrieved.");

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

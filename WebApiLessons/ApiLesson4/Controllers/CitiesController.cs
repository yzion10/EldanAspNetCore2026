using ApiLesson4.DataStores;
using ApiLesson4.DbContexts;
using ApiLesson4.DTO;
using ApiLesson4.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace ApiLesson4.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IEmailService _email;
        private readonly MainContext _context;

        public CitiesController(ILogger<CitiesController> logger, IEmailService email, MainContext context)
        {
            _logger = logger;
            _email = email;
            _context = context;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<CityDTO>> GetCities()
        //{
        //    //_logger.LogInformation("No Property here");

        //    //using (LogContext.PushProperty("SessionID", Guid.NewGuid()))
        //    //{
        //    //    //_logger.LogInformation("Getting all cities");
        //    //    _logger.LogInformation("Getting all cities from data store");
        //    //    _logger.LogInformation($"Number of cities in data store: {CitiesDataStore.Current.Count}");

        //    //    _email.Send("Cities Retrieved", $"All cities were retrieved. Total count: {CitiesDataStore.Current.Count}");

        //    //    return CitiesDataStore.Current;
        //    //}

        //    var cities = new List<CityDTO>();
        //    cities.AddRange(_context.Cities.Select(c => new CityDTO
        //    {
        //        ID = c.Id,
        //        Name = c.Name,
        //        Description = c.Description,
        //        Population = c.Population
        //    }));

        //    return Ok(cities);
        //}

        [HttpGet]
        public ActionResult<IEnumerable<CityWithoutLandMarkDTO>> GetCities()
        {
            var cities = new List<CityWithoutLandMarkDTO>();
            cities.AddRange(_context.Cities.Select(c => new CityWithoutLandMarkDTO
            {
                ID = c.Id,
                Name = c.Name,
                Description = c.Description,
                Population = c.Population
            }));

            return Ok(cities);
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

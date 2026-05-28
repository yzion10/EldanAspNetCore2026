using ApiLesson6.DataStores;
using ApiLesson6.DbContexts;
using ApiLesson6.DTO;
using ApiLesson6.Repositories;
using ApiLesson6.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Text.Json;

namespace ApiLesson6.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly IEmailService _email;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CitiesController(ILogger<CitiesController> logger, IEmailService email, ICityRepository cityRepository, IMapper mapper)
        {
            _logger = logger;
            _email = email;
            _cityRepository = cityRepository;
            _mapper = mapper;
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
        public async Task<ActionResult<IEnumerable<CityWithoutLandMarkDTO>>> GetCities(/*[FromQuery(Name = "queryame")]*/ 
            string? name, string? search, int? pageNumber, int? pageSize)
        {
            var (cities, metadata) = await _cityRepository.GetCitiesAsync(name, search, pageNumber, pageSize);

            // מיפוי city ל CityWithoutLandMarkDTO באמצעות AutoMapper
            //return Ok(_mapper.Map<List<CityWithoutLandMarkDTO>>(cities));
            
            // option 1
            //return Ok(new
            //{
            //    data = _mapper.Map<List<CityWithoutLandMarkDTO>>(cities),
            //    PagingMetadata = metadata
            //});

            // option 2 - הוספת המטה-דאטה בהדר של התגובה
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));
            return Ok(_mapper.Map<List<CityWithoutLandMarkDTO>>(cities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includeLandMarks = false)
        {
            //_logger.LogInformation($"Getting city with ID {id}");

            //var city = CitiesDataStore.
            //    Current.
            //    FirstOrDefault(c => c.ID == id);

            //if (city == null)
            //{
            //    _logger.LogWarning($"City with ID {id} not found");
            //    return NotFound();
            //}

            //_logger.LogInformation($"Returning city with ID {id}");

            //_email.Send("City Retrieved", $"City with ID {id} was retrieved.");

            //return city;

            //var city = await _cityRepository.GetCityByIdAsync(id, true);

            //if (city == null)
            //    return NotFound();

            //CityDTO cityDTO = new CityDTO
            //{
            //    ID = city.Id,
            //    Name = city.Name,
            //    Description = city.Description,
            //    Population = city.Population,
            //    LandMarks = city.LandMarks.Select(l => new LandMark
            //    {
            //        Id = l.Id,
            //        Name = l.Name,
            //        Description = l.Description
            //    }).ToList()
            //};

            // AutoMapper

            var city = await _cityRepository.GetCityByIdAsync(id, includeLandMarks);

            if(includeLandMarks)
            {
                if (city == null)
                    return NotFound();
                return Ok(_mapper.Map<CityDTO>(city));
            }
            else
            {
                if (city == null)
                    return NotFound();
                return Ok(_mapper.Map<CityWithoutLandMarkDTO>(city));
            }
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

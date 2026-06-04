using ApiLesson7.DataStores;
using ApiLesson7.DbContexts;
using ApiLesson7.DTO;
using ApiLesson7.Repositories;
using ApiLesson7.Services;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Text.Json;

namespace ApiLesson7.Controllers
{
    //[Authorize] // הוספת אטריביוט של הרשאה לכל הפעולות בקונטרולר זה
    [ApiVersion(1)]
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

        [HttpGet]
        [Authorize(Policy = "AdminOnly")] // Auth - בדיקת תפקיד המשתמש
        public async Task<ActionResult<IEnumerable<CityWithoutLandMarkDTO>>> GetCities(string? name, string? search, int? pageNumber, int? pageSize)
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
            // Auth
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if(userRole!= "Admin")
                return Forbid();

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

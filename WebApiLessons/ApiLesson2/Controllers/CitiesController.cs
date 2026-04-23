using ApiLesson2.DataStores;
using ApiLesson2.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<CityDTO> GetCities()
        {
            return CitiesDataStore.Current;
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            var city = CitiesDataStore.
                Current.
                FirstOrDefault(c => c.ID == id);

            if (city == null)
                return NotFound();

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
    }
}

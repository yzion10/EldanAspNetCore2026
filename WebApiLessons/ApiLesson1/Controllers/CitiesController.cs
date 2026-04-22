using ApiLesson1.DataStores;
using ApiLesson1.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson1.Controllers
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
    }
}

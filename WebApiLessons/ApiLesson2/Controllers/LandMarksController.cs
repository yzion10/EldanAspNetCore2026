using ApiLesson2.DataStores;
using ApiLesson2.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson2.Controllers
{
    [ApiController]
    [Route("api/cities/{cityID}/landmarks")]
    public class LandMarksController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<LandMarkDTO>> GetLandMarks(int cityID)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            return Ok(city.LandMarks);
        }

        [HttpGet("{landMarkID}")]
        public ActionResult<LandMarkDTO> GetLandMark(int cityID, int landMarkID)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            var landMark = city.LandMarks.FirstOrDefault(l => l.Id == landMarkID);

            if (landMark == null)
                return NotFound();

            return Ok(landMark);
        }
    }
}


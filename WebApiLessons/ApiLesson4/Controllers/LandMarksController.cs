using ApiLesson4.DataStores;
using ApiLesson4.DTO;
using ApiLesson4.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson4.Controllers
{
    [ApiController]
    [Route("api/cities/{cityID}/landmarks")]
    public class LandMarksController : ControllerBase
    {
        private readonly ILogger<LandMarksController> _logger;
        private readonly IEmailService _email;

        public LandMarksController(ILogger<LandMarksController> logger, IEmailService email)
        {
            // לא חובה לבדוק כי זה לא יהיה null
            // אבל זה לא יזיק לבדוק
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _email = email;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LandMark>> GetLandMarks(int cityID)
        {
            try
            {
                //throw new Exception("This is a test exception to demonstrate error handling and logging.");

                var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

                if (city == null)
                {
                    _logger.LogInformation($"City with ID {cityID} not found when trying to get landmarks");
                    return NotFound();
                }

                _logger.LogInformation($"Returning {city.LandMarks.Count()} landmarks for city with ID {cityID}");

                _email.Send("Landmarks Retrieved", $"Landmarks for city with ID {cityID} were retrieved.");

                return Ok(city.LandMarks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting landmarks for city with ID {cityID}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[HttpGet("{landMarkID}", Name = "GetLandMark")]
        [HttpGet("{landMarkID}")]
        public ActionResult<LandMark> GetLandMark(int cityID, int landMarkID)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            var landMark = city.LandMarks.FirstOrDefault(l => l.Id == landMarkID);

            if (landMark == null)
                return NotFound();

            return Ok(landMark);
        }

        [HttpPost]
        public ActionResult<LandMark> AddLandMark(int cityID, LandMarkForCreateDTO newLandMark)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            var finalLandMark = new LandMark
            {
                Id = city.LandMarks.Max(l => l.Id) + 1,
                Name = newLandMark.Name,
                Description = newLandMark.Description
            };

            ((List<LandMark>)city.LandMarks).Add(finalLandMark);

            //return Ok(finalLandMark);

            // נעדיף להחזיר 201 Created
            // עם כתובת המשאב החדש
            //return CreatedAtRoute("GetLandMark",
            //    new { cityID = cityID, landMarkID = finalLandMark.Id }, finalLandMark);

            // נעדיף להשתמש ב- CreatedAtAction שמאפשר לנו להצביע על הפעולה שמחזירה את המשאב החדש
            // במקום להגדיר שם לנתיב, נצביע על הפעולה שמחזירה את המשאב החדש
            return CreatedAtAction(nameof(GetLandMark),
                new { cityID = cityID, landMarkID = finalLandMark.Id }, finalLandMark);
        }

        [HttpPut("{landMarkID}")]
        public ActionResult UpdateLandMark(int cityID, int landMarkID,
            LandMarkForUpdateDTO updatedLandMark)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);
            if (city == null)
                return NotFound();

            var landMark = city.LandMarks.FirstOrDefault(l => l.Id == landMarkID);
            if (landMark == null)
                return NotFound();

            landMark.Name = updatedLandMark.Name ?? landMark.Name;
            landMark.Description = updatedLandMark.Description;

            return NoContent();
        }

        [HttpPatch("{landMarkID}")]
        public ActionResult PatchLandMark(int cityID, int landMarkID,
           JsonPatchDocument<LandMarkForUpdateDTO> patchDoc)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);
            if (city == null)
                return NotFound();

            var landMark = city.LandMarks.FirstOrDefault(l => l.Id == landMarkID);
            if (landMark == null)
                return NotFound();

            // Create a LandMarkForUpdateDTO from the existing LandMarkDTO
            var landMarkToPatch = new LandMarkForUpdateDTO
            {
                Name = landMark.Name,
                Description = landMark.Description
            };

            // זה ה patch doc
            patchDoc.ApplyTo(landMarkToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(landMarkToPatch))
                return BadRequest(ModelState);

            // Update the original landmark with patched values
            landMark.Name = landMarkToPatch.Name;
            landMark.Description = landMarkToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{landMarkID}")]
        public ActionResult DeleteLandMark(int cityID, int landMarkID)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);
            if (city == null)
                return NotFound();

            var landMark = city.LandMarks.FirstOrDefault(l => l.Id == landMarkID);
            if (landMark == null)
                return NotFound();

            ((List<LandMark>)city.LandMarks).Remove(landMark);
            return NoContent();
        }
    }
}


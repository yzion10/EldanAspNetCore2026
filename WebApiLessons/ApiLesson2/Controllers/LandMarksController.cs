using ApiLesson2.DataStores;
using ApiLesson2.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson2.Controllers
{
    [ApiController]
    [Route("api/cities/{cityID}/landmarks")]
    public class LandMarksController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<LandMarkDTO>> GetLandMarks(int cityID)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            return Ok(city.LandMarks);
        }

        //[HttpGet("{landMarkID}", Name = "GetLandMark")]
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

        [HttpPost]
        public ActionResult<LandMarkDTO> AddLandMark(int cityID, LandMarkForCreateDTO newLandMark)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            var finalLandMark = new LandMarkDTO
            {
                Id = city.LandMarks.Max(l => l.Id) + 1,
                Name = newLandMark.Name,
                Description = newLandMark.Description
            };

            ((List<LandMarkDTO>)city.LandMarks).Add(finalLandMark);

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
            patchDoc.ApplyTo(landMarkToPatch);

            // Update the original landmark with patched values
            landMark.Name = landMarkToPatch.Name;
            landMark.Description = landMarkToPatch.Description;

            return NoContent();
        }
    }
}


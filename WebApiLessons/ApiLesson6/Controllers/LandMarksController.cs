using ApiLesson6.DataStores;
using ApiLesson6.DTO;
using ApiLesson6.Repositories;
using ApiLesson6.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiLesson6.Controllers
{
    [ApiController]
    [Route("api/cities/{cityID}/landmarks")]
    public class LandMarksController : ControllerBase
    {
        private readonly ILogger<LandMarksController> _logger;
        private readonly IEmailService _email;
        private readonly IMapper _mapper;
        private readonly ILandMarkRepository _landMarkRepository;
        private readonly ICityRepository _cityRepository;

        public LandMarksController(ILogger<LandMarksController> logger, IEmailService email,
            IMapper mapper, ILandMarkRepository landMarkRepository, ICityRepository cityRepository)
        {
            // לא חובה לבדוק כי זה לא יהיה null
            // אבל זה לא יזיק לבדוק
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _email = email;
            _mapper = mapper;
            _landMarkRepository = landMarkRepository;
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LandMarkDto>>> GetLandMarks(int cityID)
        {
            // AutoMapper

            var city = await _cityRepository.GetCityByIdAsync(cityID, false);

            if (city == null)
                return NotFound();
            
            var landMarks = await _landMarkRepository.GetLandMarksForCityAsync(cityID);
            return Ok(_mapper.Map<IEnumerable<LandMarkDto>>(landMarks));
        }

        //[HttpGet("{landMarkID}", Name = "GetLandMark")]
        [HttpGet("{landMarkID}")]
        public async Task<ActionResult<LandMarkDto>> GetLandMark(int cityID, int landMarkID)
        {
            var city = await _cityRepository.GetCityByIdAsync(cityID, false);

            if (city == null)
                return NotFound();

            var landMark = await _landMarkRepository.GetLandMarkAsync(cityID, landMarkID);

            if (landMark == null)
                return NotFound();

            return Ok(_mapper.Map<LandMarkDto>(landMark));
        }

        [HttpPost]
        public ActionResult<LandMarkDto> AddLandMark(int cityID, LandMarkForCreateDTO newLandMark)
        {
            var city = CitiesDataStore.Current.FirstOrDefault(c => c.ID == cityID);

            if (city == null)
                return NotFound();

            var finalLandMark = new LandMarkDto
            {
                Id = city.LandMarks.Max(l => l.Id) + 1,
                Name = newLandMark.Name,
                Description = newLandMark.Description
            };

            ((List<LandMarkDto>)city.LandMarks).Add(finalLandMark);

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

            ((List<LandMarkDto>)city.LandMarks).Remove(landMark);
            return NoContent();
        }
    }
}


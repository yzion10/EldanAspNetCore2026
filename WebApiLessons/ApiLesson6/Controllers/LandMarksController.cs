using ApiLesson6.DataStores;
using ApiLesson6.DTO;
using ApiLesson6.Entities;
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
        public async Task<ActionResult<LandMarkDto>> AddLandMark(int cityID, LandMarkForCreateDTO newLandMark)
        {
            var landMarkEntity = _mapper.Map<LandMark>(newLandMark);

            await _landMarkRepository.AddLandMarkAsync(cityID, landMarkEntity);

            return CreatedAtAction(nameof(GetLandMark), new { cityID, landMarkID = landMarkEntity.Id },
                _mapper.Map<LandMarkDto>(landMarkEntity));
        }

        [HttpPut("{landMarkID}")]
        public async Task<ActionResult> UpdateLandMark(int cityID, int landMarkID, LandMarkForUpdateDTO updatedLandMark)
        {
            if (await _cityRepository.GetCityByIdAsync(cityID, false) == null)
                return NotFound();

            var landMarkEntity = await _landMarkRepository.GetLandMarkAsync(cityID, landMarkID);

            if (landMarkEntity == null)
                return NotFound();

            _mapper.Map(updatedLandMark, landMarkEntity);

            await _landMarkRepository.Save();

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
        public async Task <ActionResult> DeleteLandMark(int cityID, int landMarkID)
        {
           if(await _landMarkRepository.GetLandMarkAsync(cityID, landMarkID) == null)
                return NotFound();

            var landMartToDelete = await _landMarkRepository.GetLandMarkAsync(cityID, landMarkID);

            if (landMartToDelete == null)
                return NotFound();

            await _landMarkRepository.Delete(cityID, landMartToDelete);
            return NoContent();
        }
    }
}


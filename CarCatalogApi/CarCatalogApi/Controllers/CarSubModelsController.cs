using AutoMapper;
using CarCatalogApi.DTOs;
using CarCatalogApi.Entities;
using CarCatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarCatalogApi.Controllers;

[ApiController]
[Route("api/manufacturers/{manufacturerId}/models/{modelId}/submodels")]
public class CarSubModelsController : ControllerBase
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ICarModelRepository _carModelRepository;
    private readonly ICarSubModelRepository _carSubModelRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CarSubModelsController> _logger;
    private const string GetCarSubModelRouteName = "GetCarSubModel";
    private const string SubModelNameExistsErrorMessage = "A sub model with the same name already exists for this model";

    public CarSubModelsController(IManufacturerRepository manufacturerRepository, ICarModelRepository carModelRepository, ICarSubModelRepository carSubModelRepository, IMapper mapper, ILogger<CarSubModelsController> logger)
    {
        _manufacturerRepository = manufacturerRepository;
        _carModelRepository = carModelRepository;
        _carSubModelRepository = carSubModelRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// מחזיר את כל תת הדגמים של דגם רכב מסוים
    /// אם היצרן או הדגם לא קיימים תחזור תשובת 404 ללקוח
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarSubModelDto>>> GetSubModelsForModel(int manufacturerId, int modelId)
    {
        if (!await IsModelBelongsToManufacturerAsync(manufacturerId, modelId))
            return NotFound();

        var subModels = await _carSubModelRepository.GetSubModelsForModelAsync(modelId);
        return Ok(_mapper.Map<IEnumerable<CarSubModelDto>>(subModels));
    }

    /// <summary>
    /// מחזיר תת דגם רכב לפי מזהה
    /// </summary>
    [HttpGet("{subModelId}")]
    public async Task<ActionResult<CarSubModelDto>> GetCarSubModel(int manufacturerId, int modelId, int subModelId)
    {
        if (!await IsModelBelongsToManufacturerAsync(manufacturerId, modelId))
            return NotFound();

        var subModel = await _carSubModelRepository.GetSubModelForModelAsync(modelId, subModelId);

        if (subModel == null)
        {
            _logger.LogInformation($"Sub model with id {subModelId} for model {modelId} was not found");
            return NotFound();
        }

        return Ok(_mapper.Map<CarSubModelDto>(subModel));
    }

    /// <summary>
    /// יצירת תת דגם חדש לדגם רכב מסוים
    /// שם תת הדגם חייב להיות ייחודי בתוך הדגם, אחרת תחזור שגיאת וולידציה ללקוח
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CarSubModelDto>> CreateSubModelForModel(int manufacturerId, int modelId, CarSubModelForCreateDto subModelForCreate)
    {
        if (!await IsModelBelongsToManufacturerAsync(manufacturerId, modelId))
            return NotFound();

        if (await _carSubModelRepository.SubModelNameExistsForModelAsync(modelId, subModelForCreate.Name))
        {
            ModelState.AddModelError(nameof(subModelForCreate.Name), SubModelNameExistsErrorMessage);
            return ValidationProblem(ModelState);
        }

        var subModel = _mapper.Map<CarSubModel>(subModelForCreate);
        subModel.Name = subModel.Name.Trim();
        subModel.CarModelId = modelId;

        _carSubModelRepository.AddSubModel(subModel);
        await _carSubModelRepository.SaveChangesAsync();

        var savedSubModel = await _carSubModelRepository.GetSubModelForModelAsync(modelId, subModel.Id);
        var subModelToReturn = _mapper.Map<CarSubModelDto>(savedSubModel);

        return CreatedAtRoute(GetCarSubModelRouteName, new { manufacturerId, modelId, subModelId = subModel.Id }, subModelToReturn);
    }

    /// <summary>
    /// עדכון תת דגם קיים של דגם רכב מסוים
    /// </summary>
    [HttpPut("{subModelId}")]
    public async Task<ActionResult> UpdateSubModelForModel(int manufacturerId, int modelId, int subModelId, CarSubModelForUpdateDto subModelForUpdate)
    {
        if (!await IsModelBelongsToManufacturerAsync(manufacturerId, modelId))
            return NotFound();

        var subModel = await _carSubModelRepository.GetSubModelForModelAsync(modelId, subModelId);

        if (subModel == null)
            return NotFound();

        if (await _carSubModelRepository.SubModelNameExistsForModelAsync(modelId, subModelForUpdate.Name, subModelId))
        {
            ModelState.AddModelError(nameof(subModelForUpdate.Name), SubModelNameExistsErrorMessage);
            return ValidationProblem(ModelState);
        }

        _mapper.Map(subModelForUpdate, subModel);
        subModel.Name = subModel.Name.Trim();

        await _carSubModelRepository.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// מחיקת תת דגם קיים של דגם רכב מסוים
    /// </summary>
    [HttpDelete("{subModelId}")]
    public async Task<ActionResult> DeleteSubModelForModel(int manufacturerId, int modelId, int subModelId)
    {
        if (!await IsModelBelongsToManufacturerAsync(manufacturerId, modelId))
            return NotFound();

        var subModel = await _carSubModelRepository.GetSubModelForModelAsync(modelId, subModelId);

        if (subModel == null)
            return NotFound();

        _carSubModelRepository.DeleteSubModel(subModel);
        await _carSubModelRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// בדיקה אם הדגם שייך ליצרן, כדי למנוע גישה לדגמים של יצרנים אחרים
    /// </summary>
    private async Task<bool> IsModelBelongsToManufacturerAsync(int manufacturerId, int modelId)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return false;

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, false);

        return carModel != null;
    }
}

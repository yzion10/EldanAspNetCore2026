using AutoMapper;
using CarCatalogApi.DTOs;
using CarCatalogApi.Entities;
using CarCatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarCatalogApi.Controllers;

[ApiController]
[Route("api/manufacturers/{manufacturerId}/models")]
public class CarModelsController : ControllerBase
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ICarModelRepository _carModelRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CarModelsController> _logger;
    private const string GetCarModelRouteName = "GetCarModel";
    private const string ModelNameExistsErrorMessage = "A model with the same name already exists for this manufacturer";

    public CarModelsController(IManufacturerRepository manufacturerRepository, ICarModelRepository carModelRepository, IMapper mapper, ILogger<CarModelsController> logger)
    {
        _manufacturerRepository = manufacturerRepository;
        _carModelRepository = carModelRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// מחזיר את כל דגמי הרכב של יצרן מסוים. אם היצרן לא קיים תחזור תשובת 404 ללקוח
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarModelWithoutSubModelsDto>>> GetModelsForManufacturer(int manufacturerId)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var models = await _carModelRepository.GetModelsForManufacturerAsync(manufacturerId);
        return Ok(_mapper.Map<IEnumerable<CarModelWithoutSubModelsDto>>(models));
    }

    /// <summary>
    /// מחזיר דגם רכב לפי מזהה, עם אפשרות לכלול את תת הדגמים שלו או לא. אם היצרן או הדגם לא קיימים תחזור תשובת 404 ללקוח
    /// </summary>
    [HttpGet("{modelId}")]
    public async Task<ActionResult> GetCarModel(int manufacturerId, int modelId, bool includeSubModels = false)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
        {
            return NotFound();
        }

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, includeSubModels);

        if (carModel == null)
        {
            _logger.LogInformation("Model with id {ModelId} for manufacturer {ManufacturerId} was not found", modelId, manufacturerId);
            return NotFound();
        }

        if (includeSubModels)
        {
            return Ok(_mapper.Map<CarModelDto>(carModel));
        }

        return Ok(_mapper.Map<CarModelWithoutSubModelsDto>(carModel));
    }

    /// <summary>
    /// יצירת דגם רכב חדש ליצרן מסוים
    /// שם הדגם חייב להיות ייחודי עבור אותו יצרן, אחרת תחזור שגיאת וולידציה ללקוח
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CarModelDto>> CreateModelForManufacturer(int manufacturerId, CarModelForCreateDto carModelForCreate)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        if (await _carModelRepository.ModelNameExistsForManufacturerAsync(manufacturerId, carModelForCreate.Name))
        {
            ModelState.AddModelError(nameof(carModelForCreate.Name), ModelNameExistsErrorMessage);
            return ValidationProblem(ModelState);
        }

        var carModel = _mapper.Map<CarModel>(carModelForCreate);
        carModel.Name = carModel.Name.Trim();
        carModel.ManufacturerId = manufacturerId;

        _carModelRepository.AddModel(carModel);
        await _carModelRepository.SaveChangesAsync();

        var savedModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, carModel.Id, false);
        var carModelToReturn = _mapper.Map<CarModelDto>(savedModel);

        return CreatedAtRoute(GetCarModelRouteName, new { manufacturerId, modelId = carModel.Id }, carModelToReturn);
    }

    /// <summary>
    /// עדכון דגם רכב קיים ליצרן מסוים
    /// </summary>
    [HttpPut("{modelId}")]
    public async Task<ActionResult> UpdateModelForManufacturer(int manufacturerId, int modelId, CarModelForUpdateDto carModelForUpdate)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, false);

        if (carModel == null)
            return NotFound();

        if (await _carModelRepository.ModelNameExistsForManufacturerAsync(manufacturerId, carModelForUpdate.Name, modelId))
        {
            ModelState.AddModelError(nameof(carModelForUpdate.Name), ModelNameExistsErrorMessage);
            return ValidationProblem(ModelState);
        }

        _mapper.Map(carModelForUpdate, carModel);
        carModel.Name = carModel.Name.Trim();

        await _carModelRepository.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// מחיקת דגם רכב קיים ליצרן מסוים
    /// </summary>
    [HttpDelete("{modelId}")]
    public async Task<ActionResult> DeleteModelForManufacturer(int manufacturerId, int modelId)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, false);

        if (carModel == null)
            return NotFound();

        _carModelRepository.DeleteModel(carModel);
        await _carModelRepository.SaveChangesAsync();

        return NoContent();
    }
}

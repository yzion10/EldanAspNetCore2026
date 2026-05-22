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

    public CarModelsController(IManufacturerRepository manufacturerRepository, ICarModelRepository carModelRepository, IMapper mapper, ILogger<CarModelsController> logger)
    {
        _manufacturerRepository = manufacturerRepository;
        _carModelRepository = carModelRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarModelWithoutSubModelsDto>>> GetModelsForManufacturer(int manufacturerId)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var models = await _carModelRepository.GetModelsForManufacturerAsync(manufacturerId);
        return Ok(_mapper.Map<IEnumerable<CarModelWithoutSubModelsDto>>(models));
    }

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

    [HttpPost]
    public async Task<ActionResult<CarModelDto>> CreateModelForManufacturer(int manufacturerId, CarModelForCreateDto carModelForCreate)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        if (await _carModelRepository.ModelNameExistsForManufacturerAsync(manufacturerId, carModelForCreate.Name))
        {
            ModelState.AddModelError(nameof(carModelForCreate.Name), "A model with the same name already exists for this manufacturer.");
            return ValidationProblem(ModelState);
        }

        var carModel = _mapper.Map<CarModel>(carModelForCreate);
        carModel.Name = carModel.Name.Trim();
        carModel.ManufacturerId = manufacturerId;

        _carModelRepository.AddModel(carModel);
        await _carModelRepository.SaveChangesAsync();

        var savedModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, carModel.Id, includeSubModels: false);
        var carModelToReturn = _mapper.Map<CarModelDto>(savedModel);

        return CreatedAtRoute("GetCarModel", new { manufacturerId, modelId = carModel.Id }, carModelToReturn);
    }

    [HttpPut("{modelId}")]
    public async Task<ActionResult> UpdateModelForManufacturer(int manufacturerId, int modelId, CarModelForUpdateDto carModelForUpdate)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, includeSubModels: false);

        if (carModel == null)
            return NotFound();

        if (await _carModelRepository.ModelNameExistsForManufacturerAsync(manufacturerId, carModelForUpdate.Name, modelId))
        {
            ModelState.AddModelError(nameof(carModelForUpdate.Name), "A model with the same name already exists for this manufacturer");
            return ValidationProblem(ModelState);
        }

        _mapper.Map(carModelForUpdate, carModel);
        carModel.Name = carModel.Name.Trim();

        await _carModelRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{modelId}")]
    public async Task<ActionResult> DeleteModelForManufacturer(int manufacturerId, int modelId)
    {
        if (!await _manufacturerRepository.ManufacturerExistsAsync(manufacturerId))
            return NotFound();

        var carModel = await _carModelRepository.GetModelForManufacturerAsync(manufacturerId, modelId, includeSubModels: false);

        if (carModel == null)
            return NotFound();

        _carModelRepository.DeleteModel(carModel);
        await _carModelRepository.SaveChangesAsync();

        return NoContent();
    }
}

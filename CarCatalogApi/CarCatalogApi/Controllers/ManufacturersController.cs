using AutoMapper;
using CarCatalogApi.DTOs;
using CarCatalogApi.Entities;
using CarCatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarCatalogApi.Controllers;

[ApiController]
[Route("api/manufacturers")]
public class ManufacturersController : ControllerBase
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ManufacturersController> _logger;

    public ManufacturersController(IManufacturerRepository manufacturerRepository, IMapper mapper, ILogger<ManufacturersController> logger)
    {
        _manufacturerRepository = manufacturerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManufacturerWithoutModelsDto>>> GetManufacturers()
    {
        var manufacturers = await _manufacturerRepository.GetManufacturersAsync();
        return Ok(_mapper.Map<IEnumerable<ManufacturerWithoutModelsDto>>(manufacturers));
    }

    [HttpGet("{manufacturerId}")]
    public async Task<ActionResult> GetManufacturer(int manufacturerId, bool includeModels = false)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerAsync(manufacturerId, includeModels);

        if (manufacturer == null)
        {
            _logger.LogInformation($"Manufacturer with id {manufacturerId} was not found");
            return NotFound();
        }

        if (includeModels)
            return Ok(_mapper.Map<ManufacturerDto>(manufacturer));

        return Ok(_mapper.Map<ManufacturerWithoutModelsDto>(manufacturer));
    }

    [HttpPost]
    public async Task<ActionResult<ManufacturerDto>> CreateManufacturer(ManufacturerForCreateDto manufacturerForCreate)
    {
        if (await _manufacturerRepository.ManufacturerNameExistsAsync(manufacturerForCreate.Name))
        {
            ModelState.AddModelError(nameof(manufacturerForCreate.Name), "A manufacturer with the same name already exists");
            return ValidationProblem(ModelState); // מחזיר תשובה עם שגיאות הוולידציה ללקוח
        }

        var manufacturer = _mapper.Map<Manufacturer>(manufacturerForCreate);
        manufacturer.Name = manufacturer.Name.Trim();

        _manufacturerRepository.AddManufacturer(manufacturer);
        await _manufacturerRepository.SaveChangesAsync();

        var manufacturerToReturn = _mapper.Map<ManufacturerDto>(manufacturer);

        return CreatedAtRoute("GetManufacturer", new { manufacturerId = manufacturer.Id }, manufacturerToReturn);
    }

    [HttpPut("{manufacturerId}")]
    public async Task<ActionResult> UpdateManufacturer(int manufacturerId, ManufacturerForUpdateDto manufacturerForUpdate)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerAsync(manufacturerId, includeModels: false);

        if (manufacturer == null)
            return NotFound();

        if (await _manufacturerRepository.ManufacturerNameExistsAsync(manufacturerForUpdate.Name, manufacturerId))
        {
            ModelState.AddModelError(nameof(manufacturerForUpdate.Name), "A manufacturer with the same name already exists");
            return ValidationProblem(ModelState);
        }

        _mapper.Map(manufacturerForUpdate, manufacturer);
        manufacturer.Name = manufacturer.Name.Trim();

        await _manufacturerRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{manufacturerId}")]
    public async Task<ActionResult> DeleteManufacturer(int manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerAsync(manufacturerId, includeModels: false);

        if (manufacturer == null)
            return NotFound();

        _manufacturerRepository.DeleteManufacturer(manufacturer);
        await _manufacturerRepository.SaveChangesAsync();

        return NoContent();
    }
}

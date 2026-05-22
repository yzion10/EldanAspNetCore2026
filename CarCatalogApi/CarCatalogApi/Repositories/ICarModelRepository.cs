using CarCatalogApi.Entities;

namespace CarCatalogApi.Repositories;

public interface ICarModelRepository
{
    Task<IEnumerable<CarModel>> GetModelsForManufacturerAsync(int manufacturerId);
    Task<CarModel?> GetModelForManufacturerAsync(int manufacturerId, int modelId, bool includeSubModels);
    Task<bool> ModelNameExistsForManufacturerAsync(int manufacturerId, string name, int? ignoreModelId = null);
    void AddModel(CarModel carModel);
    void DeleteModel(CarModel carModel);
    Task<bool> SaveChangesAsync();
}

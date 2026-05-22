using CarCatalogApi.Entities;

namespace CarCatalogApi.Repositories;

public interface ICarSubModelRepository
{
    Task<IEnumerable<CarSubModel>> GetSubModelsForModelAsync(int modelId);
    Task<CarSubModel?> GetSubModelForModelAsync(int modelId, int subModelId);
    Task<bool> SubModelNameExistsForModelAsync(int modelId, string name, int? ignoreSubModelId = null);
    void AddSubModel(CarSubModel subModel);
    void DeleteSubModel(CarSubModel subModel);
    Task<bool> SaveChangesAsync();
}

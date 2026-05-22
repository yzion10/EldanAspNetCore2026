using CarCatalogApi.Data;
using CarCatalogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogApi.Repositories;

public class CarSubModelRepository : RepositoryBase, ICarSubModelRepository
{
    public CarSubModelRepository(CarCatalogDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<CarSubModel>> GetSubModelsForModelAsync(int modelId)
    {
        return await Context.CarSubModels
            .Include(subModel => subModel.CarModel)
            .Where(subModel => subModel.CarModelId == modelId)
            .OrderBy(subModel => subModel.Name)
            .ToListAsync();
    }

    public async Task<CarSubModel?> GetSubModelForModelAsync(int modelId, int subModelId)
    {
        return await Context.CarSubModels
            .Include(subModel => subModel.CarModel)
            .FirstOrDefaultAsync(subModel => subModel.CarModelId == modelId && subModel.Id == subModelId);
    }

    public async Task<bool> SubModelNameExistsForModelAsync(int modelId, string name, int? ignoreSubModelId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await Context.CarSubModels.AnyAsync(subModel =>
            subModel.CarModelId == modelId &&
            subModel.Name.ToLower() == normalizedName &&
            (!ignoreSubModelId.HasValue || subModel.Id != ignoreSubModelId.Value));
    }

    public void AddSubModel(CarSubModel subModel)
    {
        Context.CarSubModels.Add(subModel);
    }

    public void DeleteSubModel(CarSubModel subModel)
    {
        Context.CarSubModels.Remove(subModel);
    }
}

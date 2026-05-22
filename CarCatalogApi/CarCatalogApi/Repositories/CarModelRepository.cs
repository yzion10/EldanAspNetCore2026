using CarCatalogApi.Data;
using CarCatalogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogApi.Repositories;

public class CarModelRepository : RepositoryBase, ICarModelRepository
{
    public CarModelRepository(CarCatalogDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<CarModel>> GetModelsForManufacturerAsync(int manufacturerId)
    {
        return await Context.CarModels
            .Include(model => model.Manufacturer)
            .Where(model => model.ManufacturerId == manufacturerId)
            .OrderBy(model => model.Name)
            .ToListAsync();
    }

    public async Task<CarModel?> GetModelForManufacturerAsync(int manufacturerId, int modelId, bool includeSubModels)
    {
        var query = Context.CarModels
            .Include(model => model.Manufacturer)
            .Where(model => model.ManufacturerId == manufacturerId);

        if (includeSubModels)
            query = query.Include(model => model.SubModels);

        return await query.FirstOrDefaultAsync(model => model.Id == modelId);
    }

    public async Task<bool> ModelNameExistsForManufacturerAsync(int manufacturerId, string name, int? ignoreModelId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await Context.CarModels.AnyAsync(model =>
            model.ManufacturerId == manufacturerId &&
            model.Name.ToLower() == normalizedName &&
            (!ignoreModelId.HasValue || model.Id != ignoreModelId.Value));
    }

    public void AddModel(CarModel carModel)
    {
        Context.CarModels.Add(carModel);
    }

    public void DeleteModel(CarModel carModel)
    {
        Context.CarModels.Remove(carModel);
    }
}

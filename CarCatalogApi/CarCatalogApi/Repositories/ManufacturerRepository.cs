using CarCatalogApi.Data;
using CarCatalogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogApi.Repositories;

/// <summary>
/// מחלקה שאחראית לרכז את הלוגיקה של יצרנים מול בסיס הנתונים
/// באמצעות Entity Framework
/// </summary>
public class ManufacturerRepository : RepositoryBase, IManufacturerRepository
{
    public ManufacturerRepository(CarCatalogDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Manufacturer>> GetManufacturersAsync()
    {
        return await Context.Manufacturers
            .OrderBy(manufacturer => manufacturer.Name)
            .ToListAsync();
    }

    public async Task<Manufacturer?> GetManufacturerAsync(int manufacturerId, bool includeModels)
    {
        var query = Context.Manufacturers.AsQueryable();

        if (includeModels)
            query = query.Include(manufacturer => manufacturer.Models).
                    ThenInclude(model => model.SubModels);

        return await query.FirstOrDefaultAsync(manufacturer => manufacturer.Id == manufacturerId);
    }

    public async Task<bool> ManufacturerExistsAsync(int manufacturerId)
    {
        return await Context.Manufacturers.AnyAsync(manufacturer => manufacturer.Id == manufacturerId);
    }

    public async Task<bool> ManufacturerNameExistsAsync(string name, int? ignoreManufacturerId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await Context.Manufacturers.AnyAsync(manufacturer =>
            manufacturer.Name.ToLower() == normalizedName &&
            (!ignoreManufacturerId.HasValue || manufacturer.Id != ignoreManufacturerId.Value));
    }

    public void AddManufacturer(Manufacturer manufacturer)
    {
        Context.Manufacturers.Add(manufacturer);
    }

    public void DeleteManufacturer(Manufacturer manufacturer)
    {
        Context.Manufacturers.Remove(manufacturer);
    }
}

using CarCatalogApi.Entities;

namespace CarCatalogApi.Repositories;

public interface IManufacturerRepository
{
    Task<IEnumerable<Manufacturer>> GetManufacturersAsync();
    Task<Manufacturer?> GetManufacturerAsync(int manufacturerId, bool includeModels);
    Task<bool> ManufacturerExistsAsync(int manufacturerId);
    Task<bool> ManufacturerNameExistsAsync(string name, int? ignoreManufacturerId = null);
    void AddManufacturer(Manufacturer manufacturer);
    void DeleteManufacturer(Manufacturer manufacturer);
    Task<bool> SaveChangesAsync();
}

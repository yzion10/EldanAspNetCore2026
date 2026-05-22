namespace CarCatalogApi.DTOs;

public class CarModelWithoutSubModelsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? YearFrom { get; set; }
    public string? BodyType { get; set; }
    public int ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }
}

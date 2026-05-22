namespace CarCatalogApi.DTOs;

public class CarModelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? YearFrom { get; set; }
    public string? BodyType { get; set; }
    public int ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }
    public ICollection<CarSubModelDto> SubModels { get; set; } = new List<CarSubModelDto>();
    public int SubModelsCount => SubModels.Count;
}

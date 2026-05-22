namespace CarCatalogApi.DTOs;

public class ManufacturerWithoutModelsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
}

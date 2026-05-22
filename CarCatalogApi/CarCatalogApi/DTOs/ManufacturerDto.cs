namespace CarCatalogApi.DTOs;

public class ManufacturerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
    public ICollection<CarModelWithoutSubModelsDto> Models { get; set; } = new List<CarModelWithoutSubModelsDto>();
    public int ModelsCount => Models.Count;
}

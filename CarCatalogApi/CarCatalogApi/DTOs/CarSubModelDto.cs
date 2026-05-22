namespace CarCatalogApi.DTOs;

public class CarSubModelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? EngineCode { get; set; }
    public int? HorsePower { get; set; }
    public int CarModelId { get; set; }
    public string? CarModelName { get; set; }
}

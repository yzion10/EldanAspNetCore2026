using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.Entities;

public class CarSubModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? EngineCode { get; set; }

    public int? HorsePower { get; set; }

    public int CarModelId { get; set; }

    public CarModel CarModel { get; set; } = null!;
}

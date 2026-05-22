using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.DTOs;

public class CarSubModelForCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? EngineCode { get; set; }

    [Range(1, 5000)]
    public int? HorsePower { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.DTOs;

public class CarModelForUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1800, 2100)]
    public int? YearFrom { get; set; }

    [MaxLength(50)]
    public string? BodyType { get; set; }
}

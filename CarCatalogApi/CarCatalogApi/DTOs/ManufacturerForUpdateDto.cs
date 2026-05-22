using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.DTOs;

public class ManufacturerForUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Country { get; set; }

    [Range(1800, 2100)]
    public int? FoundedYear { get; set; }
}

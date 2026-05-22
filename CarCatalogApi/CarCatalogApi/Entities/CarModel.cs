using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.Entities;

public class CarModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int? YearFrom { get; set; }

    [MaxLength(50)]
    public string? BodyType { get; set; }

    public int ManufacturerId { get; set; }

    public Manufacturer Manufacturer { get; set; } = null!;

    public ICollection<CarSubModel> SubModels { get; set; } = new List<CarSubModel>();
}

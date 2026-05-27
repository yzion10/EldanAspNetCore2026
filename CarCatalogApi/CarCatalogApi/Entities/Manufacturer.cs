using System.ComponentModel.DataAnnotations;

namespace CarCatalogApi.Entities;

/// <summary>
/// יצרנים - מייצג את הטבלה בבסיס הנתונים
/// </summary>
public class Manufacturer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Country { get; set; }

    public int? FoundedYear { get; set; }

    public ICollection<CarModel> Models { get; set; } = new List<CarModel>();
}

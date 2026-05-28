using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLesson6.Entities
{
    public class LandMark
    {
        public LandMark(string name)
        {
            Name = name;
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        //[ForeignKey("City")] // מפתוח לאובייקט City בכדי ליצור קשר בין הטבלאות
        public int CityId { get; set; }

        // ניווט לאובייקט City, מאפשר גישה נוחה לפרטי העיר מהאובייקט Landmark
        public City City { get; set; }

        public bool IsDeleted { get; set; }
    }
}

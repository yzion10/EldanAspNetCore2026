using System.ComponentModel.DataAnnotations;

namespace ApiLesson4.DTO
{
    public class LandMarkForCreateDTO
    {
        public string? Name { get; set; }

        [MaxLength(10, ErrorMessage = "The description cannot be longer than 10 characters")]
        public string Description { get; set; }
    }

    public class LandMarkForUpdateDTO
    {
        public string? Name { get; set; }

        [MaxLength(10, ErrorMessage = "The description cannot be longer than 10 characters")]
        public string Description { get; set; }
    }
}

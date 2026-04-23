namespace ApiLesson2.DTO
{
    public class LandMarkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class LandMarkForCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

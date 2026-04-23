namespace ApiLesson2.DTO
{
    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public IEnumerable<LandMarkDTO> LandMarks { get; set; } = new List<LandMarkDTO>();
        public int LandMarksCount => LandMarks.Count();
    }
}

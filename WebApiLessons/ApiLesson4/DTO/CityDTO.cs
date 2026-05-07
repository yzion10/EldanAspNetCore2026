namespace ApiLesson4.DTO
{
    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public IEnumerable<LandMark> LandMarks { get; set; } = new List<LandMark>();
        public int LandMarksCount => LandMarks.Count();
    }
}

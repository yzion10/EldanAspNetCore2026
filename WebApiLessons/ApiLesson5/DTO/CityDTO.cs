namespace ApiLesson5.DTO
{
    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Population { get; set; }

        public IEnumerable<LandMarkDto> LandMarks { get; set; } = new List<LandMarkDto>();
        public int LandMarksCount => LandMarks.Count();
    }

    public class CityWithoutLandMarkDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Population { get; set; }
    }
}

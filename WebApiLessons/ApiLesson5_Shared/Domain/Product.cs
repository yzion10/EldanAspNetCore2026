namespace ApiLesson5_Shared.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Notes { get; set; } = string.Empty;

        public List<Feature> Features { get; set; } = new List<Feature>();
    }
}

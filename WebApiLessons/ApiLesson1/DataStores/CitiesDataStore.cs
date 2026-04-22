using ApiLesson1.DTO;

namespace ApiLesson1.DataStores
{
    public static class CitiesDataStore
    {
        public static List<CityDTO> Current { get; set; } = new List<CityDTO>();

        static CitiesDataStore()
        {
            Current = new List<CityDTO>()
            {
                new CityDTO{ID = 1, Name = "Tel-Aviv"},
                new CityDTO{ID = 2, Name = "Jerusalem"},
            };
        }
    }
}

using ApiLesson4.DTO;

namespace ApiLesson4.DataStores
{
    public static class CitiesDataStore
    {
        public static List<CityDTO> Current { get; set; } = new List<CityDTO>();

        static CitiesDataStore()
        {
            Current = new List<CityDTO>()
            {
                new CityDTO{ID = 1, Name = "Tel-Aviv", LandMarks = new List<LandMark>
                {
                    new LandMark{Id = 1, Name = "Azrieli", Description = "A shopping mall"},
                    new LandMark{Id = 2, Name = "Rothschild", Description = "A street"}
                }
                },
                new CityDTO{ID = 2, Name = "Jerusalem", LandMarks = new List<LandMark>
                {
                    new LandMark{Id = 3, Name = "Western Wall", Description = "A religious site"},
                    new LandMark{Id = 4, Name = "Dome of the Rock", Description = "An Islamic shrine"}
                }},
            };
        }
    }
}

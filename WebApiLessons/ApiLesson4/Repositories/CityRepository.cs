using ApiLesson4.DbContexts;
using ApiLesson4.DTO;
using ApiLesson4.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiLesson4.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MainContext _context;

        public CityRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<ICollection<City>> GetCitiesAsync()
        {
            //return await _context.Cities.OrderByDescending(c => c.Name).ToListAsync();
            return await _context.Cities.OrderBy(c => c.Population).ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int id, bool includeLandMarks)
        {
            if (includeLandMarks)
                return await _context.Cities.Include(c => c.LandMarks).FirstOrDefaultAsync(c => c.Id == id); // זה בעצם join בין city ל landmark

            return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        }
    }

    public interface ICityRepository
    {
        Task<ICollection<City>> GetCitiesAsync();
        Task<City?> GetCityByIdAsync(int id, bool includeLandMarks);
    }
}

using ApiLesson6.DbContexts;
using ApiLesson6.DTO;
using ApiLesson6.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiLesson6.Repositories
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

        public async Task<ICollection<City>> GetCitiesAsync(string? name, string? search)
        {
            var cities = _context.Cities.AsQueryable();
            //var cities = _context.Cities.ToList(); // לא יעיל - מעלה את כל הערים ואז בזיכרון עושה עליו את התחימות


            if (!string.IsNullOrEmpty(name))
                cities = cities.Where(c => c.Name.Equals(name));

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                cities = cities.Where(c => c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));
            }

            return await cities.OrderByDescending(c => c.Name).ToListAsync();
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
        Task<ICollection<City>> GetCitiesAsync(string? name);
        Task<City?> GetCityByIdAsync(int id, bool includeLandMarks);
    }
}

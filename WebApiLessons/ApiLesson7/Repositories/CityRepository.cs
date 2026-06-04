using ApiLesson7.DbContexts;
using ApiLesson7.DTO;
using ApiLesson7.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiLesson7.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MainContext _context;
        const int maxPageSize = 10;

        public CityRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<ICollection<City>> GetCitiesAsync()
        {
            //return await _context.Cities.OrderByDescending(c => c.Name).ToListAsync();
            return await _context.Cities.OrderBy(c => c.Population).ToListAsync();
        }

        // Tuple - מבנה נתונים שמכיל כמה ערכים שונים, במקרה הזה רשימת ערים ומטה-דאטה של עמודים
        public async Task<(ICollection<City> Cities, PagingMetadata PagingMetadata)> GetCitiesAsync(
            string? name, string? search,
            int? pageNumber = 1, int? pageSize = maxPageSize)
        {
            var cities = _context.Cities.AsQueryable();
            //var cities = _context.Cities.ToList(); // לא יעיל - מעלה את כל הערים ואז בזיכרון עושה עליו את התחימות

            if (pageSize > maxPageSize)
                pageSize = maxPageSize; // הגבלת מספר הערים שמוחזרות ללקוח

            if (!string.IsNullOrEmpty(name))
                cities = cities.Where(c => c.Name.Equals(name));

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                cities = cities.Where(c => c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));
            }

            var totalItemCount = await cities.CountAsync();
            var metadata = new PagingMetadata(totalItemCount, pageSize ?? maxPageSize, pageNumber ?? 1);

            cities = cities.
                            Skip(((pageNumber ?? 1) - 1) * (pageSize ?? maxPageSize)).
                            Take(pageSize ?? maxPageSize);

            return (await cities.OrderByDescending(c => c.Name).ToListAsync(), metadata);
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
        Task<(ICollection<City> Cities, PagingMetadata PagingMetadata)> GetCitiesAsync(string? name, string? search, int? pageNumber, int? pageSize);
        Task<City?> GetCityByIdAsync(int id, bool includeLandMarks);
    }
}

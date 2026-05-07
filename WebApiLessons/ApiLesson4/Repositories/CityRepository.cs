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
            return await _context.Cities.ToListAsync();
        }
    }

    public interface ICityRepository
    {
        Task<ICollection<City>> GetCitiesAsync();
    }
}

using ApiLesson6.DbContexts;
using ApiLesson6.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiLesson6.Repositories
{
    public class LandMarkRepository : ILandMarkRepository
    {
        private readonly MainContext _context;

        public LandMarkRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LandMark>> GetLandMarksForCityAsync(int cityId)
        {
            return await _context.LandMarks.Where(l => l.CityId == cityId).ToListAsync();
        }

        public async Task<LandMark?> GetLandMarkAsync(int cityId, int landMarkId)
        {
            return await _context.LandMarks.FirstOrDefaultAsync(l => l.CityId == cityId && l.Id == landMarkId);
        }

        public async Task AddLandMarkAsync(int cityId, LandMark landMark)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);

            if (city != null)
                city.LandMarks.Add(landMark);
        }

        public async Task Delete(int cityId, LandMark landMark)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);

            if (city != null)
                city.LandMarks.Remove(landMark);
        }
    }
}

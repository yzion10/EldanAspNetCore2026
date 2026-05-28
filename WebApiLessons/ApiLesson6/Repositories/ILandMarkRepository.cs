using ApiLesson6.Entities;

namespace ApiLesson6.Repositories
{
    public interface ILandMarkRepository
    {
        Task AddLandMarkAsync(int cityId, LandMark landMark);
        Task<LandMark?> GetLandMarkAsync(int cityId, int landMarkId);
        Task<IEnumerable<LandMark>> GetLandMarksForCityAsync(int cityId);
    }
}
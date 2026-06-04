using ApiLesson7.Entities;

namespace ApiLesson7.Repositories
{
    public interface ILandMarkRepository
    {
        Task AddLandMarkAsync(int cityId, LandMark landMark, bool autoSave = true);
        Task<LandMark?> GetLandMarkAsync(int cityId, int landMarkId);
        Task<IEnumerable<LandMark>> GetLandMarksForCityAsync(int cityId);
        Task Delete(int cityId, LandMark landMark, bool autoSave = true);
        Task Save();
    }
}
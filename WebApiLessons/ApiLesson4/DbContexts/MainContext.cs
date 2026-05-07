using Microsoft.EntityFrameworkCore;

namespace ApiLesson4.DbContexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.City> Cities { get; set; }
        public DbSet<Entities.LandMark> LandMarks { get; set; }
    }
}

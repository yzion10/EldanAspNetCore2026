using Microsoft.EntityFrameworkCore;

namespace ApiLesson6.DbContexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.City> Cities { get; set; }
        public DbSet<Entities.LandMark> LandMarks { get; set; }

        /// <summary>
        /// הגדרת המודל של ה-DbContext,
        /// ניתן להגדיר את הקשרים בין הטבלאות, אילו שדות חובה, אילו שדות ייחודיים וכו'
        /// הוספת נתונים לטבלאות וכו
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.City>().
                HasData(
                    new Entities.City("New York") { Id = 1, Description = "The city that never sleeps", Population = 8000000 },
                    new Entities.City("Paris") { Id = 2, Description = "The city of love", Population = 2000000 },
                    new Entities.City("Tokyo") {Id = 3, Description = "The city of the rising sun", Population = 9000000 }
                );

            modelBuilder.Entity<Entities.LandMark>().
                HasData(
                    new Entities.LandMark("Statue of Liberty") { Id = 1, CityId = 1, Description = "A symbol of freedom" },
                    new Entities.LandMark("Eiffel Tower") { Id = 2, CityId = 2, Description = "A global cultural icon of France" },
                    new Entities.LandMark("Tokyo Tower") { Id = 3, CityId = 3, Description = "A communications and observation tower" }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}

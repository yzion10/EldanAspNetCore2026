using CarCatalogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogApi.Data;

public class CarCatalogDbContext : DbContext
{
    public CarCatalogDbContext(DbContextOptions<CarCatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<CarModel> CarModels => Set<CarModel>();
    public DbSet<CarSubModel> CarSubModels => Set<CarSubModel>();

    /// <summary>
    /// הגדרת המודל והקשרים בין הטבלאות
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100); // שם היצרן הוא שדה חובה עם אורך מקסימלי של 100 תווים
            entity.Property(m => m.Country).HasMaxLength(100); // שדה מדינה עם אורך מקסימלי של 100 תווים
            entity.HasIndex(m => m.Name).IsUnique(); // יצירת אינדקס ייחודי על שם היצרן כדי למנוע יצירת יצרנים עם שמות כפולים

            entity.HasMany(m => m.Models) // יצרן יכול להיות קשור להרבה דגמים
                .WithOne(m => m.Manufacturer) // כל דגם שייך ליצרן אחד
                .HasForeignKey(m => m.ManufacturerId)
                .OnDelete(DeleteBehavior.Cascade); // מחיקת יצרן תמחק גם את כל הדגמים שלו וכל תת-הדגמים שלהם
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100); // שם הדגם הוא שדה חובה עם אורך מקסימלי של 100 תווים
            entity.Property(m => m.BodyType).HasMaxLength(50); // שדה סוג גוף עם אורך מקסימלי של 50 תווים
            entity.HasIndex(m => new { m.ManufacturerId, m.Name }).IsUnique(); // יצירת אינדקס ייחודי על שילוב של מזהה היצרן ושם הדגם כדי למנוע יצירת דגמים עם שמות כפולים לאותו יצרן

            entity.HasMany(m => m.SubModels)
                .WithOne(sm => sm.CarModel)
                .HasForeignKey(sm => sm.CarModelId)
                .OnDelete(DeleteBehavior.Cascade); // מחיקת דגם תמחק גם את כל תת-הדגמים שלו
        });

        modelBuilder.Entity<CarSubModel>(entity =>
        {
            entity.Property(sm => sm.Name).IsRequired().HasMaxLength(100); // שם תת-הדגם הוא שדה חובה עם אורך מקסימלי של 100 תווים
            entity.Property(sm => sm.EngineCode).HasMaxLength(50);
            entity.HasIndex(sm => new { sm.CarModelId, sm.Name }).IsUnique(); // יצירת אינדקס ייחודי על שילוב של מזהה הדגם ושם תת-הדגם כדי למנוע יצירת תת-דגמים עם שמות כפולים לאותו דגם
        });

        // הוספת נתוני דמה לטבלאות
        InsertData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// הוספת נתוני דמה לטבלאות
    /// קיימת בדיקה אם הנתונים כבר קיימים כדי למנוע הוספה כפולה בעת הרצת המיגרציות
    /// </summary>
    private static void InsertData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer { Id = 1, Name = "Chevrolet", Country = "United States", FoundedYear = 1911 },
            new Manufacturer { Id = 2, Name = "Toyota", Country = "Japan", FoundedYear = 1937 }
        );

        modelBuilder.Entity<CarModel>().HasData(
            new CarModel { Id = 1, Name = "Corvette", ManufacturerId = 1, YearFrom = 1953, BodyType = "Sports Car" },
            new CarModel { Id = 2, Name = "Camaro", ManufacturerId = 1, YearFrom = 1966, BodyType = "Muscle Car" },
            new CarModel { Id = 3, Name = "Corolla", ManufacturerId = 2, YearFrom = 1966, BodyType = "Sedan" }
        );

        modelBuilder.Entity<CarSubModel>().HasData(
            new CarSubModel { Id = 1, Name = "LT2", CarModelId = 1, EngineCode = "6.2L V8", HorsePower = 490 },
            new CarSubModel { Id = 2, Name = "Z06", CarModelId = 1, EngineCode = "5.5L V8", HorsePower = 670 },
            new CarSubModel { Id = 3, Name = "SS", CarModelId = 2, EngineCode = "6.2L V8", HorsePower = 455 },
            new CarSubModel { Id = 4, Name = "Hybrid", CarModelId = 3, EngineCode = "1.8L Hybrid", HorsePower = 138 }
        );
    }
}

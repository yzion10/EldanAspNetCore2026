using CarCatalogApi.Data;
using CarCatalogApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.AddProblemDetails();

        builder.Services.AddAutoMapper(_ => { }, typeof(Program));

        builder.Services.AddDbContext<CarCatalogDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
        });

        builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
        builder.Services.AddScoped<ICarSubModelRepository, CarSubModelRepository>();

        var app = builder.Build();

        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

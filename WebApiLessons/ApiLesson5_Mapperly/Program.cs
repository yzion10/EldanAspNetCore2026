
using ApiLesson5_Mapperly.Mappings;
using ApiLesson5_Shared.Repositories;

namespace ApiLesson5_Mapperly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Register the product storage service as a singleton
            builder.Services.AddSingleton<IProductStorage, ProductStorage>();

            // Register the ProductMapper as a singleton
            builder.Services.AddSingleton<ProductMapper>();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

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
}

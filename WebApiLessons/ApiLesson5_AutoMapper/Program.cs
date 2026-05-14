
using ApiLesson5_Shared.Repositories;

namespace ApiLesson5_AutoMapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Register the product storage service as a singleton
            builder.Services.AddSingleton<IProductStorage, ProductStorage>();

            // Register AutoMapper and scan the current assembly for profiles
            // license: https://automapper.org/docs/10.1.1/license.html
            builder.Services.AddAutoMapper(_ => { }, typeof(Program));

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

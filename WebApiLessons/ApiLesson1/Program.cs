

namespace ApiLesson1
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
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //app.Run(c=> c.Response.WriteAsync("Yosi Login..."));

            //app.Run(async c =>
            //{
            //    await c.Response.WriteAsync("Login...");
            //    //await context.Response.WriteAsync("Hello World!");
            //});

            //app.Run(async c =>
            //{
            //    await c.Response.WriteAsync("Hello World!");
            //});

            app.Run();
        }
    }
}

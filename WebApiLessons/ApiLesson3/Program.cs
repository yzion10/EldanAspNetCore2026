
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

namespace ApiLesson3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Minute)
                .CreateBootstrapLogger();

            // DI - Dependency Injection

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // ניקוי כל הלוגים - לא נשתמש בזה
            // נשתמש בזה רק אם נרצה לעבוד מול מערכת לוגים צד שלישי כמו לדוגמא Serilog או NLog
            //builder.Logging.ClearProviders();

            // Add services to the container.
            builder.Services.AddControllers(o =>
            {
                // אם הלקוח מבקש פורמט לא נתמך נחזיר 406 Not Acceptable
                //o.ReturnHttpNotAcceptable = true; 
            })
            .AddXmlDataContractSerializerFormatters()
            .AddNewtonsoftJson(); // אם נרצה להשתמש ב-NewtonsoftJson במקום ב-System.Text.Json

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddProblemDetails(o =>
            //{
            //    o.CustomizeProblemDetails = (context) =>
            //    {
            //        context.ProblemDetails.Extensions.Add("AppName", "ApiLesson2");

            //        // רק בפיתוח נוסיף את שם המכונה ל-ProblemDetails
            //        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            //            context.ProblemDetails.Extensions.Add("MachineName", Environment.MachineName);
            //    };
            //});

            builder.Services.AddProblemDetails();

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // במצב פרודקשן נרצה לטפל בשגיאות בצורה מרכזית במקום להראות את הסטאק טרייס ללקוח
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

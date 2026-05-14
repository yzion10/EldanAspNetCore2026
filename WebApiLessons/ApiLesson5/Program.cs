
using ApiLesson5.DbContexts;
using ApiLesson5.Repositories;
using ApiLesson5.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ApiLesson5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string template = "{Timestamp:dd-MM-yyyy} [{MachineName}-{ThreadId}] ({RequestId}) {Message}{NewLine}{Properties}{NewLine}{NewLine}";

            // Serilog
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
                //.WriteTo.Console()
                .WriteTo.Console(outputTemplate: template)
                //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} [{MachineName}] (Thread {ThreadId})] {SourceContext} - {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Minute, outputTemplate: template)
                //.Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .CreateLogger();

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

            // AutoMapper
            // license : https://automapper.org/#license
            builder.Services.AddAutoMapper(_ => { }, typeof(Program));

            //#if DEBUG
            //builder.Services.AddTransient<IEmailService, DevelopmentEmailService>();
            //builder.Services.AddSingleton<IEmailService, DevelopmentEmailService>();
            builder.Services.AddScoped<IEmailService, DevelopmentEmailService>();
            //#else
            //builder.Services.AddTransient<IEmailService, ProductionEmailService>();
            //#endif

            // AddTransient - כל פעם שנשתמש באובייקט יווצר מופע חדש של כל האובייקט
            // AddScoped - כל פעם שנשתמש בקונטרולר יווצר מופע חדש של כל האובייקט
            // כשנפנה לקונטרולר אחר יווצר מופע חדש של כל האובייקט
            // AddSingleton - כל פעם שנשתמש באובייקט נקבל את אותו מופע של כל האובייקט. נוצר פעם אחת בזיכרון

            // ApiLesson5
            //---------------------------------------------------------------------------------

            // נרצה להשתמש ב-AddDbContext במקום ב-AddScoped מכיוון ש-AddDbContext כבר מטפל בעצמו במחזור החיים של ה-DbContext ומוודא שהוא מתאים לסביבה של ה-Web API
            builder.Services.AddDbContext<MainContext>(optionsAction =>
            {
                optionsAction.UseSqlite("Data Source=YosiDB.db");
            });

            // כדי להשתמש בפקודות של EF Core נצטרך להתקין את הכלי של EF Core
            // הרצה חד פעמית
            // dotnet tool install -g dotnet-ef

            // הרצה של כל המיגרציות על בסיס הנתונים
            // נצטרך להריץ בכל פרויקט בו משתמשים ב eff
            // dotnet ef migrations add InitialDB

            // עדכון בסיס הנתונים על בסיס המיגרציות שנוצרו
            // dotnet ef database update

            // dotnet ef database update --migration "InitialDB" - כדי לעדכן את בסיס הנתונים למיגרציה ספציפית

            
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<ILandMarkRepository, LandMarkRepository>();

            var app = builder.Build();

            // הרצת המיגרציות אוטומטית עם הרצת האפליקציה
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MainContext>();
                dbContext.Database.Migrate();
            }

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

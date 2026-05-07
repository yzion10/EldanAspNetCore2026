
using Microsoft.Data.Sqlite;

namespace ApiLesson4_MinApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            string connectionString = "Data Source=yosidb.db";

            InitDB(connectionString);

            app.MapGet("/", () => "this is my api");

            // זה לא בטיחותי
            // אם אני אשלח בקשה ל /users?userName=admin' OR '1'='1
            // מעבירים פרמטרים
            app.MapGet("/users/unsafe", (string userName) =>
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Users WHERE Username = '{userName}'";

                    using var reader = command.ExecuteReader(); 

                    var users = new List<User>();

                    if (reader.Read())
                    {
                        users.Add(new User(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(4)
                        ));
                    }

                    if (users.Count > 0)
                        return Results.Ok(users);
                    else
                        return Results.NotFound();
                }
            });

            app.MapGet("/users/safe", (string userName) =>
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Users WHERE Username = @userName";
                    command.Parameters.AddWithValue("@userName", userName);
                    using var reader = command.ExecuteReader();
                    var users = new List<User>();
                    if (reader.Read())
                    {
                        users.Add(new User(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(4)
                        ));
                    }
                    if (users.Count > 0)
                        return Results.Ok(users);
                    else
                        return Results.NotFound();
                }
            });

            app.Run();
        }

        public static void InitDB(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using var command = connection.CreateCommand();

                command.CommandText = """

                    DROP TABLE IF EXISTS Users;
                    
                    CREATE TABLE Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        PasswordHash TEXT NOT NULL,
                        IsAdmin BOOLEAN NOT NULL DEFAULT 0
                    );
                    
                        INSERT INTO
                            Users
                        (Username, Email, PasswordHash, IsAdmin)
                        VALUES
                            ('admin', 'admin@example.com', 'HAHHBJSDDSDS', 1),
                            ('yosi', 'yosi@example.com', 'ABCDFHSOKDJK', 0)
                    """;

                command.ExecuteNonQuery();
            }
        }

        record User(int Id, string Username, string Email, bool IsAdmin);
    }
}

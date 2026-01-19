using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ORM.Services;

namespace Arrowbuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services registrieren
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // DbContext mit ConnectionString aus Configuration
            builder.Services.AddDbContext<DbManager>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = GetConnectionString(configuration);
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            var app = builder.Build();

            // Datenbank initialisieren
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbManager>();

                if (app.Environment.IsDevelopment())
                {
                    dbContext.Database.EnsureCreated();
                }
                else
                {
                    dbContext.Database.Migrate();
                }
            }

            // HTTP-Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            // Priorität: Umgebungsvariablen > appsettings
            var connStr = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(connStr) && !connStr.Contains("Password=;"))
            {
                return connStr;
            }

            // Aus Umgebungsvariablen bauen
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Arrowbuilder";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("DB_PASSWORD fehlt!");
            }

            return $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
        }
    }
}
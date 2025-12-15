using Microsoft.AspNetCore.Builder;
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

            // TODO: DbContext registrieren, sobald der Typ verfügbar ist:
            // builder.Services.AddDbContext<DBManager>(options => { /* konfigurieren */ });
            builder.Services.AddDbContext<DbManager>();

            var app = builder.Build();

            // HTTP-Pipeline konfigurieren
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

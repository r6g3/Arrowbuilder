using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ORM.Services;

public class DbManagerFactory : IDesignTimeDbContextFactory<DbManager>
{
    public DbManager CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbManager>();

        // Dummy Connection String nur für Design-Time (Migrations erstellen)
        // Die echte Connection String kommt aus appsettings.json zur Runtime
        var connectionString = "Server=localhost;Port=3306;Database=ArrowbuilderDB;User Id=root;Password=AnotherRootSecret!;";

        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.Parse("8.0.40-mysql"),
            options => options.EnableRetryOnFailure()
        );

        return new DbManager(optionsBuilder.Options, null!);
    }
}
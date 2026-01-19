using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Arrowbuilder.Models;

namespace ORM.Services;

public class DbManager : DbContext
{
    private readonly IConfiguration? _configuration;

    // Constructor für DI (Produktion)
    public DbManager(DbContextOptions<DbManager> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    // Parameterloser Constructor für Migrations
    public DbManager() { }

    public DbSet<User> Users { get; set; }
    public DbSet<Shaft> Shafts { get; set; }
    public DbSet<Nock> Nocks { get; set; }
    public DbSet<Fletching> Fletchings { get; set; }
    public DbSet<Point> Points { get; set; }
    public DbSet<Insert> Inserts { get; set; }
    public DbSet<Arrow> Arrows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Priorität: 1. Umgebungsvariablen, 2. Connection String, 3. Fallback
            var connectionString = BuildConnectionString();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    private string BuildConnectionString()
    {
        // 1. Versuche Connection String aus appsettings
        var connStr = _configuration?.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(connStr))
        {
            return connStr;
        }

        // 2. Baue aus Umgebungsvariablen (Docker/Hosting)
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
        var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Arrowbuilder";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Datenbank-Passwort fehlt! Setze DB_PASSWORD Umgebungsvariable oder ConnectionString in appsettings.json");
        }

        return $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
    }
}
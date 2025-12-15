using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace ORM.Services;

public class DbManager : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Shaft> Shafts { get; set; }
    public DbSet<Nock> Nocks { get; set; }
    public DbSet<Fletching> Fletchings { get; set; }
    public DbSet<Models.Point> Points { get; set; }
    public DbSet<Insert> Inserts { get; set; }
    public DbSet<Arrow> Arrows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Env-Variablen lesen (mit Defaults, falls lokal ohne Docker)
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
        var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "shopdb";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD"); // kein Default für Sicherheit

        // Optional: Early validation, damit Fehlkonfiguration schnell auffällt
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Environment-Variable 'DB_PASSWORD' ist nicht gesetzt.");
        }

        var connectionString =
            $"Server={host};Port={port};Database={database};User Id={user};Password={password};";

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

}
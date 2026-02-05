using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TravelCleanArch.Infrastructure.Persistence;

public  class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();


        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)    
            .AddEnvironmentVariables()
            .Build();

        var cs = config.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not found. Ensure configuration providers (appsettings.json or environment variables) are available and the EnvironmentVariables package is referenced.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(cs)
            .Options;

        return new AppDbContext(options);
    }
}

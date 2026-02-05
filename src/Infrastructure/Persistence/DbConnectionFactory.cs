using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace TravelCleanArch.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public sealed class DbConnectionFactory(IConfiguration config) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var cs = config.GetConnectionString("DefaultConnection")
                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        return new NpgsqlConnection(cs);
    }
}

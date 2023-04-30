using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace MODEL;

public class QueryContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public QueryContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration!.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}

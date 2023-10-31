using Microsoft.Extensions.Configuration;
using MODEL.Extensions;
using Npgsql;
using System.Data;

namespace MODEL;

public class QueryContext
{
    private readonly IConfiguration _configuration;

    public QueryContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_configuration.GetPostgresConnectionString());
}

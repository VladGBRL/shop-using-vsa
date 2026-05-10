using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuestApp.Backend.Shared;

public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") 
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}

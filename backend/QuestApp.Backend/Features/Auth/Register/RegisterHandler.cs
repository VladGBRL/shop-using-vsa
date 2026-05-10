using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Auth.Register;

public class RegisterHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public RegisterHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> HandleAsync(RegisterCommand command)
    {
        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        
        var checkEmailSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
        using var checkCommand = new SqlCommand(checkEmailSql, connection);
        checkCommand.Parameters.AddWithValue("@Email", command.Email);
        
        var exists = (int)(await checkCommand.ExecuteScalarAsync() ?? 0) > 0;
        if (exists)
        {
            return false; 
        }

        
        var passwordHash = HashPassword(command.Password);

        
        var insertSql = @"
            INSERT INTO Users (Username, Email, PasswordHash, CreatedAt) 
            VALUES (@Username, @Email, @PasswordHash, GETUTCDATE())";
            
        using var insertCommand = new SqlCommand(insertSql, connection);
        insertCommand.Parameters.AddWithValue("@Username", command.Username);
        insertCommand.Parameters.AddWithValue("@Email", command.Email);
        insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);

        var rowsAffected = await insertCommand.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}

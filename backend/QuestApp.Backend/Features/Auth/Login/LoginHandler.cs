using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Auth.Login;

public class LoginHandler
{
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(SqlConnectionFactory connectionFactory, JwtTokenGenerator jwtTokenGenerator)
    {
        _connectionFactory = connectionFactory;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResult> HandleAsync(LoginCommand command)
    {
        var passwordHash = HashPassword(command.Password);

        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        var sql = "SELECT Id, Username FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
        using var sqlCommand = new SqlCommand(sql, connection);
        sqlCommand.Parameters.AddWithValue("@Email", command.Email);
        sqlCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);

        using var reader = await sqlCommand.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var userId = reader.GetInt32(0);
            var username = reader.GetString(1);

            var token = _jwtTokenGenerator.GenerateToken(userId, command.Email, username);

            return new LoginResult
            {
                Success = true,
                Token = token,
                Username = username,
                Message = "Login successful"
            };
        }

        return new LoginResult
        {
            Success = false,
            Message = "Invalid email or password"
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}

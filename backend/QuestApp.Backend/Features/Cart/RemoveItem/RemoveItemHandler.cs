using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Cart.RemoveItem;

public class RemoveItemHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public RemoveItemHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> HandleAsync(int userId, int productId)
    {
        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        const string sql = "DELETE FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@ProductId", productId);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}

using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Cart.AddItem;

public class AddItemHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public AddItemHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task HandleAsync(int userId, AddItemCommand command)
    {
        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        
        const string checkSql = "SELECT Id, Quantity FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
        using var checkCmd = new SqlCommand(checkSql, connection);
        checkCmd.Parameters.AddWithValue("@UserId", userId);
        checkCmd.Parameters.AddWithValue("@ProductId", command.ProductId);

        using var reader = await checkCmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            
            int existingId = reader.GetInt32(0);
            int existingQty = reader.GetInt32(1);
            reader.Close();

            const string updateSql = "UPDATE CartItems SET Quantity = @Quantity WHERE Id = @Id";
            using var updateCmd = new SqlCommand(updateSql, connection);
            updateCmd.Parameters.AddWithValue("@Quantity", existingQty + command.Quantity);
            updateCmd.Parameters.AddWithValue("@Id", existingId);
            await updateCmd.ExecuteNonQueryAsync();
        }
        else
        {
            reader.Close();

            
            const string insertSql = """
                INSERT INTO CartItems (UserId, ProductId, Quantity, AddedAt)
                VALUES (@UserId, @ProductId, @Quantity, @AddedAt)
                """;
            using var insertCmd = new SqlCommand(insertSql, connection);
            insertCmd.Parameters.AddWithValue("@UserId", userId);
            insertCmd.Parameters.AddWithValue("@ProductId", command.ProductId);
            insertCmd.Parameters.AddWithValue("@Quantity", command.Quantity);
            insertCmd.Parameters.AddWithValue("@AddedAt", DateTime.UtcNow);
            await insertCmd.ExecuteNonQueryAsync();
        }
    }
}

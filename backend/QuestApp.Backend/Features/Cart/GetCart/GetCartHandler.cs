using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Cart.GetCart;

public class GetCartHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public GetCartHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<CartItemDto>> HandleAsync(GetCartQuery query)
    {
        var items = new List<CartItemDto>();

        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        const string sql = """
            SELECT ci.Id, ci.ProductId, p.Name AS ProductName, p.Price, p.ImageUrl, ci.Quantity, ci.AddedAt
            FROM CartItems ci
            JOIN Products p ON p.Id = ci.ProductId
            WHERE ci.UserId = @UserId
            ORDER BY ci.AddedAt DESC
            """;

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", query.UserId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            items.Add(new CartItemDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                AddedAt = reader.GetDateTime(reader.GetOrdinal("AddedAt"))
            });
        }

        return items;
    }
}

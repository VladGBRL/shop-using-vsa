using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Products.GetProducts;

public class GetProductsHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public GetProductsHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<ProductDto>> HandleAsync(GetProductsQuery query)
    {
        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        var sql = @"
            SELECT p.Id, p.Name, p.Description, p.Price, p.Stock, p.ImageUrl, c.Name as CategoryName 
            FROM Products p 
            JOIN Categories c ON c.Id = p.CategoryId";
            
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        var products = new List<ProductDto>();

        while (await reader.ReadAsync())
        {
            products.Add(new ProductDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
            });
        }

        return products;
    }
}

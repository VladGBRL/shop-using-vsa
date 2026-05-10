using Microsoft.Data.SqlClient;
using QuestApp.Backend.Shared;

namespace QuestApp.Backend.Features.Checkout;

public class CheckoutHandler
{
    private readonly SqlConnectionFactory _connectionFactory;

    public CheckoutHandler(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CheckoutResult?> HandleAsync(int userId, CheckoutCommand command)
    {
        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            
            var cartItems = new List<(int ProductId, int Quantity)>();

            const string cartSql = "SELECT ProductId, Quantity FROM CartItems WHERE UserId = @UserId";
            using (var cartCmd = new SqlCommand(cartSql, connection, transaction))
            {
                cartCmd.Parameters.AddWithValue("@UserId", userId);
                using var reader = await cartCmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    cartItems.Add((reader.GetInt32(0), reader.GetInt32(1)));
                }
            }

            if (cartItems.Count == 0)
                return null; 

            
            var orderLines = new List<(int ProductId, int Quantity, decimal UnitPrice)>();

            foreach (var (productId, quantity) in cartItems)
            {
                const string priceSql = "SELECT Price FROM Products WHERE Id = @ProductId";
                using var priceCmd = new SqlCommand(priceSql, connection, transaction);
                priceCmd.Parameters.AddWithValue("@ProductId", productId);

                var priceObj = await priceCmd.ExecuteScalarAsync();
                if (priceObj is null) continue; 

                decimal unitPrice = (decimal)priceObj;
                orderLines.Add((productId, quantity, unitPrice));
            }

            decimal totalPrice = PriceCalculator.CalculateTotal(orderLines.Select(ol => (ol.Quantity, ol.UnitPrice)));

            
            const string orderSql = """
                INSERT INTO Orders (UserId, TotalPrice, ShippingAddress, Status, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @TotalPrice, @ShippingAddress, 'Pending', @CreatedAt)
                """;

            int orderId;
            using (var orderCmd = new SqlCommand(orderSql, connection, transaction))
            {
                orderCmd.Parameters.AddWithValue("@UserId", userId);
                orderCmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                orderCmd.Parameters.AddWithValue("@ShippingAddress", command.ShippingAddress);
                orderCmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                orderId = (int)(await orderCmd.ExecuteScalarAsync())!;
            }

            
            foreach (var (productId, quantity, unitPrice) in orderLines)
            {
                
                const string itemSql = """
                    INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
                    VALUES (@OrderId, @ProductId, @Quantity, @Price)
                    """;
                using var itemCmd = new SqlCommand(itemSql, connection, transaction);
                itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                itemCmd.Parameters.AddWithValue("@ProductId", productId);
                itemCmd.Parameters.AddWithValue("@Quantity", quantity);
                itemCmd.Parameters.AddWithValue("@Price", unitPrice);
                await itemCmd.ExecuteNonQueryAsync();

                
                const string stockSql = "UPDATE Products SET Stock = Stock - @Quantity WHERE Id = @ProductId";
                using var stockCmd = new SqlCommand(stockSql, connection, transaction);
                stockCmd.Parameters.AddWithValue("@Quantity", quantity);
                stockCmd.Parameters.AddWithValue("@ProductId", productId);
                await stockCmd.ExecuteNonQueryAsync();
            }

            
            const string clearCartSql = "DELETE FROM CartItems WHERE UserId = @UserId";
            using (var clearCmd = new SqlCommand(clearCartSql, connection, transaction))
            {
                clearCmd.Parameters.AddWithValue("@UserId", userId);
                await clearCmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return new CheckoutResult
            {
                OrderId = orderId,
                TotalPrice = totalPrice,
                Message = "Order placed successfully."
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

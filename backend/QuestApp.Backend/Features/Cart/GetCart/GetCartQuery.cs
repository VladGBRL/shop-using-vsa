namespace QuestApp.Backend.Features.Cart.GetCart;

public record GetCartQuery(int UserId);

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}

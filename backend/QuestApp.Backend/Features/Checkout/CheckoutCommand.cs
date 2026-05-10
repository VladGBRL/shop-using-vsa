namespace QuestApp.Backend.Features.Checkout;

public record CheckoutCommand(string ShippingAddress);

public class CheckoutResult
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Message { get; set; } = string.Empty;
}

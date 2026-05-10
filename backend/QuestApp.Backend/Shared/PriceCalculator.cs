namespace QuestApp.Backend.Shared;

public static class PriceCalculator
{
    public static decimal CalculateTotal(IEnumerable<(int Quantity, decimal UnitPrice)> items)
    {
        return items.Sum(item => item.Quantity * item.UnitPrice);
    }
}

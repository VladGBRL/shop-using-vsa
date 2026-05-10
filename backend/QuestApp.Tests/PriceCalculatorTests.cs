using QuestApp.Backend.Shared;

namespace QuestApp.Tests;

public class PriceCalculatorTests
{
    [Fact]
    public void CalculateTotal_ShouldReturnCorrectSum()
    {
        
        var items = new List<(int Quantity, decimal UnitPrice)>
        {
            (2, 10.50m), 
            (1, 5.00m),  
            (3, 1.00m)   
        };

        
        var total = PriceCalculator.CalculateTotal(items);

        
        Assert.Equal(29.00m, total);
    }

    [Fact]
    public void CalculateTotal_EmptyList_ShouldReturnZero()
    {
        
        var items = new List<(int Quantity, decimal UnitPrice)>();

        
        var total = PriceCalculator.CalculateTotal(items);

        
        Assert.Equal(0, total);
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Checkout;

public static class CheckoutEndpoint
{
    public static void MapCheckoutEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/checkout", async (HttpContext context, CheckoutCommand command, CheckoutHandler handler) =>
        {
            if (context.Items["UserId"] is not int userId)
                return Results.Unauthorized();

            if (string.IsNullOrWhiteSpace(command.ShippingAddress))
                return Results.BadRequest(new { Message = "ShippingAddress is required." });

            var result = await handler.HandleAsync(userId, command);

            if (result is null)
                return Results.BadRequest(new { Message = "Cannot place an order with an empty cart." });

            return Results.Ok(result);
        })
        .WithName("Checkout")
        .WithTags("Checkout");
    }
}

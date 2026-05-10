using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Cart.RemoveItem;

public static class RemoveItemEndpoint
{
    public static void MapRemoveItemEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/cart/{productId:int}", async (HttpContext context, int productId, RemoveItemHandler handler) =>
        {
            if (context.Items["UserId"] is not int userId)
                return Results.Unauthorized();

            var removed = await handler.HandleAsync(userId, productId);
            return removed
                ? Results.Ok(new { Message = "Item removed from cart." })
                : Results.NotFound(new { Message = "Item not found in cart." });
        })
        .WithName("RemoveCartItem")
        .WithTags("Cart");
    }
}

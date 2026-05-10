using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Cart.AddItem;

public static class AddItemEndpoint
{
    public static void MapAddItemEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/cart", async (HttpContext context, AddItemCommand command, AddItemHandler handler) =>
        {
            if (context.Items["UserId"] is not int userId)
                return Results.Unauthorized();

            await handler.HandleAsync(userId, command);
            return Results.Ok(new { Message = "Item added to cart." });
        })
        .WithName("AddCartItem")
        .WithTags("Cart");
    }
}

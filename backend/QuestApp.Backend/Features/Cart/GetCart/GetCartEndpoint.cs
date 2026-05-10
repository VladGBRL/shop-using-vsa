using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Cart.GetCart;

public static class GetCartEndpoint
{
    public static void MapGetCartEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/cart", async (HttpContext context, GetCartHandler handler) =>
        {
            if (context.Items["UserId"] is not int userId)
                return Results.Unauthorized();

            var items = await handler.HandleAsync(new GetCartQuery(userId));
            return Results.Ok(items);
        })
        .WithName("GetCart")
        .WithTags("Cart");
    }
}

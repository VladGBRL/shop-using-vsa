using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace QuestApp.Backend.Features.Products.GetProducts;

public static class GetProductsEndpoint
{
    public static void MapGetProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async ([FromServices] GetProductsHandler handler) =>
        {
            var query = new GetProductsQuery();
            var products = await handler.HandleAsync(query);
            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .WithTags("Products");
    }
}

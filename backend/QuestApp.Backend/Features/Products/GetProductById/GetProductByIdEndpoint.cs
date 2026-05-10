using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace QuestApp.Backend.Features.Products.GetProductById;

public static class GetProductByIdEndpoint
{
    public static void MapGetProductByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id:int}", async (int id, [FromServices] GetProductByIdHandler handler) =>
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await handler.HandleAsync(query);
            
            if (product == null)
            {
                return Results.NotFound(new { Message = $"Product with ID {id} not found." });
            }
            
            return Results.Ok(product);
        })
        .WithName("GetProductById")
        .WithTags("Products");
    }
}

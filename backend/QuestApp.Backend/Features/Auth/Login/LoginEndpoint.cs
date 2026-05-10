using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (LoginCommand command, LoginHandler handler) =>
        {
            var result = await handler.HandleAsync(command);
            
            if (!result.Success)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(result);
        })
        .WithName("LoginUser")
        .WithTags("Auth");
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace QuestApp.Backend.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (RegisterCommand command, RegisterHandler handler) =>
        {
            var success = await handler.HandleAsync(command);
            
            if (!success)
            {
                return Results.Conflict(new { Message = "Email is already registered." });
            }

            return Results.Ok(new { Message = "User registered successfully." });
        })
        .WithName("RegisterUser")
        .WithTags("Auth");
    }
}

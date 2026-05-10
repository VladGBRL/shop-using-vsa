namespace QuestApp.Backend.Features.Auth.Login;

public class LoginCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Username { get; set; }
    public string? Message { get; set; }
}
using Microsoft.Extensions.Configuration;
using Moq;
using QuestApp.Backend.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace QuestApp.Tests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidJwtString()
    {
        
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsAValidSecretKeyForTesting1234567890!");
        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var generator = new JwtTokenGenerator(mockConfig.Object);

        
        var token = generator.GenerateToken(1, "test@email.com", "testuser");

        
        Assert.False(string.IsNullOrWhiteSpace(token));
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        
        Assert.Equal("TestIssuer", jsonToken.Issuer);
        Assert.Contains(jsonToken.Audiences, a => a == "TestAudience");
        Assert.Equal("1", jsonToken.Subject);
        Assert.Equal("test@email.com", jsonToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
    }
}

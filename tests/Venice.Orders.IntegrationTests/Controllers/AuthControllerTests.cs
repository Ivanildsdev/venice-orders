using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Venice.Orders.Api.Models;
using Venice.Orders.IntegrationTests.Helpers;

namespace Venice.Orders.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<IntegrationTestBase>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestBase _factory;

    public AuthControllerTests(IntegrationTestBase factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidUsername_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginRequest { Username = "testuser", Password = "testpass" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        loginResponse.Should().NotBeNull();
        loginResponse!.Token.Should().NotBeNullOrEmpty();
        loginResponse.ExpiresIn.Should().BeGreaterThan(0);
    }
}


using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Venice.Orders.Api.Models;
using Venice.Orders.Application.DTOs;
using Venice.Orders.IntegrationTests.Helpers;

namespace Venice.Orders.IntegrationTests.Controllers;

public class OrdersControllerTests : IClassFixture<IntegrationTestBase>
{
    private readonly IntegrationTestBase _factory;

    public OrdersControllerTests(IntegrationTestBase factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        return _factory.CreateClient();
    }

    private string GetTestToken()
    {
        return "test-token";
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var client = CreateClient();
        var token = GetTestToken();
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Date = DateTime.UtcNow.AddSeconds(-1),
            Items = new List<OrderItemRequestDto>
            {
                new OrderItemRequestDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 10.50m
                }
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/orders", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status {response.StatusCode}: {errorContent}");
        }
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        order.Should().NotBeNull();
        order!.Total.Should().Be(21.00m);
        order.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateOrder_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var client = CreateClient();
        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Date = DateTime.UtcNow.AddSeconds(-1),
            Items = new List<OrderItemRequestDto>
            {
                new OrderItemRequestDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 10.50m
                }
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/orders", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetOrderById_WithValidId_ShouldReturnOrder()
    {
        // Arrange
        var client = CreateClient();
        var token = GetTestToken();
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        var createRequest = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Date = DateTime.UtcNow.AddSeconds(-1),
            Items = new List<OrderItemRequestDto>
            {
                new OrderItemRequestDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 3,
                    UnitPrice = 15.00m
                }
            }
        };

        var createResponse = await client.PostAsJsonAsync("/api/orders", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderDto>();

        // Act
        var getResponse = await client.GetAsync($"/api/orders/{createdOrder!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var order = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
        order.Should().NotBeNull();
        order!.Id.Should().Be(createdOrder.Id);
        order.Total.Should().Be(45.00m);
    }
}


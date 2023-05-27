using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace UniversityAccessControl.Tests.Integration;

public class SubjectIntegrationTest : IClassFixture<AccessControlWebApplicationFactory<Program>>
{
    private readonly AccessControlWebApplicationFactory<Program> _factory;

    public SubjectIntegrationTest(AccessControlWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostSubject()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");

        // Act
        var response = await client.PostAsJsonAsync("/Subject", new
        {
            firstName = "John",
            middleName = "Johny",
            lastName = "Doe",
            dateOfBirth = "2023-12-30",
            groupIds = Array.Empty<int>()
        });

        // Assert
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync())!;
        result["firstName"]!.GetValue<string>().Should().Be("John");
        result["middleName"]!.GetValue<string>().Should().Be("Johny");
        result["lastName"]!.GetValue<string>().Should().Be("Doe");
        result["dateOfBirth"]!.GetValue<string>().Should().Be("2023-12-30");
        ((JsonArray)result["groups"]!).Should().BeEmpty();
    }
}
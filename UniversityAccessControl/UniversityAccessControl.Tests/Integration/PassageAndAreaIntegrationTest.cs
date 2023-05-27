using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace UniversityAccessControl.Tests.Integration;

public class PassageAndAreaIntegrationTest : IClassFixture<AccessControlWebApplicationFactory<Program>>
{
    private readonly AccessControlWebApplicationFactory<Program> _factory;

    public PassageAndAreaIntegrationTest(AccessControlWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostAndPutPassageWithArea()
    {
        var client = _factory.CreateClient().Authorized();

        var area1PostResponse = await client.PostAsJsonAsync("/Area", new { name = "area 1" });
        area1PostResponse.EnsureSuccessStatusCode();
        var area1Id =
            JsonSerializer.Deserialize<JsonNode>(await area1PostResponse.Content.ReadAsStringAsync())!["id"]!
                .GetValue<int>();
        var area2PostResponse = await client.PostAsJsonAsync("/Area", new { name = "area 2" });
        area2PostResponse.EnsureSuccessStatusCode();
        var area2Id =
            JsonSerializer.Deserialize<JsonNode>(await area2PostResponse.Content.ReadAsStringAsync())!["id"]!
                .GetValue<int>();

        var passagePostResponse = await client.PostAsJsonAsync("/Passage", new { name = "Passage 1", areaId = area1Id });
        passagePostResponse.EnsureSuccessStatusCode();
        var postResult = JsonSerializer.Deserialize<JsonNode>(await passagePostResponse.Content.ReadAsStringAsync())!;
        postResult["name"]!.GetValue<string>().Should().Be("Passage 1");
        postResult["area"]!["id"]!.GetValue<int>().Should().Be(area1Id);
        postResult["area"]!["name"]!.GetValue<string>().Should().Be("area 1");

        var passagePutResponse =
            await client.PutAsJsonAsync("/Passage/1", new { id = 1, name = "Door 1", areaId = area2Id });
        passagePutResponse.EnsureSuccessStatusCode();

        var getPassageResult = (await client.GetFromJsonAsync<JsonNode>("/Passage/1"))!;
        getPassageResult["name"]!.GetValue<string>().Should().Be("Door 1");
        getPassageResult["area"]!["id"]!.GetValue<int>().Should().Be(area2Id);
        getPassageResult["area"]!["name"]!.GetValue<string>().Should().Be("area 2");
    }
}
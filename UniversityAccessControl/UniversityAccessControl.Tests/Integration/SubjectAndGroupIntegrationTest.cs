using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace UniversityAccessControl.Tests.Integration;

public class SubjectAndGroupIntegrationTest : IClassFixture<AccessControlWebApplicationFactory<Program>>
{
    private readonly AccessControlWebApplicationFactory<Program> _factory;

    public SubjectAndGroupIntegrationTest(AccessControlWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostAndPutSubjectWithGroup()
    {
        var client = _factory.CreateClient().Authorized();

        var group1PostResponse = await client.PostAsJsonAsync("/Group", new { name = "group 1" });
        group1PostResponse.EnsureSuccessStatusCode();
        var group1Id =
            JsonSerializer.Deserialize<JsonNode>(await group1PostResponse.Content.ReadAsStringAsync())!["id"]!
                .GetValue<int>();

        var group2PostResponse = await client.PostAsJsonAsync("/Group", new { name = "group 2" });
        group2PostResponse.EnsureSuccessStatusCode();
        var group2Id =
            JsonSerializer.Deserialize<JsonNode>(await group2PostResponse.Content.ReadAsStringAsync())!["id"]!
                .GetValue<int>();

        var subjectPostResponse = await client.PostAsJsonAsync("/Subject", new
        {
            firstName = "John",
            middleName = "Johny",
            lastName = "Doe",
            dateOfBirth = "2023-12-30",
            groupIds = new[] { group1Id }
        });
        subjectPostResponse.EnsureSuccessStatusCode();
        var postResult = JsonSerializer.Deserialize<JsonNode>(await subjectPostResponse.Content.ReadAsStringAsync())!;
        postResult["firstName"]!.GetValue<string>().Should().Be("John");
        postResult["middleName"]!.GetValue<string>().Should().Be("Johny");
        postResult["lastName"]!.GetValue<string>().Should().Be("Doe");
        postResult["dateOfBirth"]!.GetValue<string>().Should().Be("2023-12-30");
        postResult["groups"]![0]!["id"]!.GetValue<int>().Should().Be(1);
        postResult["groups"]![0]!["name"]!.GetValue<string>().Should().Be("group 1");

        var subjectPutResponse = await client.PutAsJsonAsync("/Subject/1", new
        {
            id = 1,
            firstName = "Dave",
            middleName = "Johny",
            lastName = "Doe",
            dateOfBirth = "2023-12-30",
            groupIds = new[] { group1Id, group2Id }
        });
        subjectPutResponse.EnsureSuccessStatusCode();

        var getSubjectResult = (await client.GetFromJsonAsync<JsonNode>("/Subject/1"))!;
        getSubjectResult["firstName"]!.GetValue<string>().Should().Be("Dave");
        ((JsonArray)getSubjectResult["groups"]!).Should().HaveCount(2);
        getSubjectResult["groups"]![0]!["id"]!.GetValue<int>().Should().Be(1);
        getSubjectResult["groups"]![0]!["name"]!.GetValue<string>().Should().Be("group 1");
        getSubjectResult["groups"]![1]!["id"]!.GetValue<int>().Should().Be(2);
        getSubjectResult["groups"]![1]!["name"]!.GetValue<string>().Should().Be("group 2");
    }
}
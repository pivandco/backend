using System.Net.Http.Headers;

namespace UniversityAccessControl.Tests.Integration;

public static class HttpClientExtensions
{
    public static HttpClient Authorized(this HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");
        return client;
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using ProjetoTerra.API;
using Xunit;

namespace Api.Controllers;

public class TestsController : IDisposable
{
    private readonly TestServer _server;
    private readonly HttpClient _client;
    private const string RepositoryName = "teste";

    public TestsController()
    {
        var builder = new WebHostBuilder()
            .UseStartup<Program>();

        _server = new TestServer(builder);
        _client = _server.CreateClient();
    }
    
    [Fact]
    public async Task Get_Endpoint_ReturnsSuccessAndExpectedResponse()
    {
        var response = await _client.GetAsync($"/api/github/branchs/{RepositoryName}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("ExpectedData", content);
    }

    public void Dispose()
    {
        _server.Dispose();
        _client.Dispose();
    }
}
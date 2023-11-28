using Cynnamon.Controllers;
using Cynnamon.Models;
using System.Net;
using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

public class TheaterManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();


    [Fact]
    public async Task Theater1_CreateTheater() {
        var response = await _httpClient.PostAsJsonAsync(
            "/Theater", 
            new AddTheaterRequest(
                Name: "Sydney Opera House",
                Location: "Sydney, Australia", 
                Seats: 5738));

        Assert.Multiple(() => {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        });
    }
}

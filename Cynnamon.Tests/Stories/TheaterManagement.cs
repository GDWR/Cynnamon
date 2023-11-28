using Cynnamon.Models;
using System.Net;
using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

public class TheaterManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();


    [Fact]
    public async Task Theater1_CreateTheater()
    {
        var request = new AddTheaterRequest(Name: "Sydney Opera House", Location: "Sydney, Australia", Seats: 5738);
        var response = await _httpClient.PostAsJsonAsync("/Theater", request);

        var createdTheater = await response.Content.ReadFromJsonAsync<Theater>();
        
        Assert.Multiple(() => {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdTheater);
            Assert.Equal(createdTheater.Name, request.Name);
            Assert.Equal(createdTheater.Location, request.Location);
            Assert.Equal(createdTheater.Seats, request.Seats);
        });
    }
}

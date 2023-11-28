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
    
    [Fact]
    public async Task Theater2_UpdateAnExistingTheater() {
        var createRequest = new AddTheaterRequest(Name: "Sydney Opera House", Location: "Sydney, Australia", Seats: 5738);
        var createResponse = await _httpClient.PostAsJsonAsync("/Theater", createRequest);

        var createdTheater = await createResponse.Content.ReadFromJsonAsync<Theater>();
        Assert.NotNull(createdTheater);

        var updateRequest = new UpdateTheaterRequest(Name: "Sydney Opera House", Location: "Sydney, Australia", Seats: 5738);
        var response = await _httpClient.PatchAsJsonAsync($"/Theater/{createdTheater.Id}", updateRequest);

        Assert.Multiple(() => {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });
    }
}

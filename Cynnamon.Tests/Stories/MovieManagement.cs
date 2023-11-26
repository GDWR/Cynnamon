using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

record Movie(string Title, string Description, string Duration, string Genre);

[Collection("Sequential")]
public class MovieManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Movie1_Get_All_Movies() {
        var response = await _httpClient.GetAsync("/movie");
        response.EnsureSuccessStatusCode();

        var movies = await response.Content.ReadFromJsonAsync<Movie[]>();
        Assert.NotNull(movies);
    }
}

using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;


[Collection("Sequential")]
public class MovieManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Movie1_GetAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        response.EnsureSuccessStatusCode();

        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        Assert.NotNull(movies);
    }
}

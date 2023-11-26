using System.Net;
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

    [Fact]
    public async Task Movie2_AddNewMovie() {
        var movie = new Movie("The Matrix", "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.", "2h 16m", "Action, Sci-Fi");
        var response = await _httpClient.PostAsJsonAsync("/movie", movie);
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

[Collection("MovieManagement")]
public class MovieManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();
    private readonly AddMovieRequest _testMovieRequest = new(
            "The Bee Movie",
            "Barry B. Benson, a bee just graduated from college, is disillusioned at his lone career choice: making honey. On a special trip outside the hive, Barry's life is saved by Vanessa, a florist in New York City. As their relationship blossoms, he discovers humans actually eat honey, and subsequently decides to sue them.",
            "1h 31m",
            "Animation, Adventure, Comedy"
        );

    [Fact]
    public async Task Movie1_GetAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        response.EnsureSuccessStatusCode();

        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        Assert.NotNull(movies);
    }

    [Fact]
    public async Task Movie2_AddNewMovie() {
        var response = await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Movie2_NewMovieNowContainedInAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        response.EnsureSuccessStatusCode();

        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        Assert.NotNull(movies);
        Assert.Contains(_testMovieRequest.Title, movies.Select(m => m.Title));
    }
}

using System.Net;
using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

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
        foreach (var _ in Enumerable.Range(0, 10)) {
            await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        }
        
        var response = await _httpClient.GetAsync("/movie");
        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        Assert.Multiple(() => {
            Assert.NotNull(movies);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });
    }

    [Fact]
    public async Task Movie2_AddNewMovie() {
        var response = await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Movie2_NewMovieNowContainedInAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        
        Assert.Multiple(() => {
            Assert.NotNull(movies);
            Assert.Contains(_testMovieRequest.Title, movies.Select(m => m.Title));
        });
    }

    [Fact]
    public async Task Movie3_UpdateExistingMovie()
    {
        var request = await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        var movie = request.Content.ReadFromJsonAsync<Movie>();

        var patchRequest = new PatchMovieRequest(null, "Updated description", null, null);
        var patchResponse = await _httpClient.PatchAsJsonAsync($"/movie/{movie.Id}", patchRequest);

        var movieRequest = await _httpClient.GetAsync("/movie/1");
        var updatedMovie = await movieRequest.Content.ReadFromJsonAsync<Movie>();

        Assert.Multiple(() => {
            Assert.NotNull(movie);
            Assert.Equal(HttpStatusCode.OK,patchResponse.StatusCode);
            Assert.Equal(_testMovieRequest.Title, updatedMovie.Title);
            Assert.Equal("Updated description", updatedMovie.Description);
            Assert.Equal(_testMovieRequest.Duration, updatedMovie.Duration);
            Assert.Equal(_testMovieRequest.Genre, updatedMovie.Genre);
        });
    }
    
    
    [Fact]
    public async Task Movie4_DeleteExistingMovie()
    {
        var request = await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        var movie = request.Content.ReadFromJsonAsync<Movie>();
        
        // Making assumption we are using incrementing ids from 1, may change in the future.
        var deleteRequest = await _httpClient.DeleteAsync($"/movie/{movie.Id}");
        var getRequest = await _httpClient.GetAsync($"/movie/{movie.Id}");

        Assert.Multiple(() => {
            Assert.Equal(HttpStatusCode.OK, deleteRequest.StatusCode);
            Assert.Equal(HttpStatusCode.Gone, getRequest.StatusCode);
        });
    }
}

using System.Net;
using System.Net.Http.Json;

namespace Cynnamon.Tests.Stories;

[TestCaseOrderer(SequenceTestCaseOrderer.TypeName, SequenceTestCaseOrderer.AssemblyName)]
public class MovieManagement(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>> {
    private readonly HttpClient _httpClient = factory.CreateClient();
    private readonly AddMovieRequest _testMovieRequest = new(
            "The Bee Movie",
            "Barry B. Benson, a bee just graduated from college, is disillusioned at his lone career choice: making honey. On a special trip outside the hive, Barry's life is saved by Vanessa, a florist in New York City. As their relationship blossoms, he discovers humans actually eat honey, and subsequently decides to sue them.",
            "1h 31m",
            "Animation, Adventure, Comedy"
        );

    [Fact, TestIndex(0)]
    public async Task Movie1_GetAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        Assert.Multiple(() => {
            Assert.NotNull(movies);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });
    }

    [Fact, TestIndex(1)]
    public async Task Movie2_AddNewMovie() {
        var response = await _httpClient.PostAsJsonAsync("/movie", _testMovieRequest);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact, TestIndex(2)]
    public async Task Movie2_NewMovieNowContainedInAllMovies() {
        var response = await _httpClient.GetAsync("/movie");
        var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
        
        Assert.Multiple(() => {
            Assert.NotNull(movies);
            Assert.Contains(_testMovieRequest.Title, movies.Select(m => m.Title));
        });
    }

    [Fact, TestIndex(3)]
    public async Task Movie3_UpdateExistingMovie()
    {
        // Making assumption we are using incrementing ids from 1, may change in the future.
        var patchRequest = new PatchMovieRequest(null, "Updated description", null, null);
        await _httpClient.PatchAsJsonAsync("/movie/1", patchRequest);

        var movieRequest = await _httpClient.GetAsync("/movie/1");
        var movie = await movieRequest.Content.ReadFromJsonAsync<Movie>();

        Assert.Multiple(() => {
            Assert.NotNull(movie);
            Assert.Equal(_testMovieRequest.Title, movie.Title);
            Assert.Equal("Updated description", movie.Description);
            Assert.Equal(_testMovieRequest.Duration, movie.Duration);
            Assert.Equal(_testMovieRequest.Genre, movie.Genre);
        });
    }
}

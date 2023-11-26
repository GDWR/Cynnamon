using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(opt => opt.UseInMemoryDatabase("Cynnamon"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/movie", async (Database db) => TypedResults.Ok(await db.Movies.ToListAsync()))
    .WithOpenApi()
    .WithDescription("Get all movies");

app.MapGet("/movie/{id:int}", async Task<Results<Ok<Movie>, NotFound, StatusCodeHttpResult>> (Database db, int id) => 
        await db.Movies.FindAsync(id) switch {
            null => TypedResults.NotFound(),
            {  Deleted: true } => TypedResults.StatusCode(StatusCodes.Status410Gone),
            var movie => TypedResults.Ok(movie),
        }
    )
    .WithOpenApi()
    .WithDescription("Get movie by id");

app.MapPost("/movie", async (Database db, AddMovieRequest movieRequest) => {
        var movie = new Movie(
            null,
            movieRequest.Title,
            movieRequest.Description,
            movieRequest.Duration,
            movieRequest.Genre
        );

        await db.AddAsync(movie);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/movie/{movie.Id}", movie);
    })
    .WithDescription("Add a new movie");

app.MapPatch("/movie/{id}", async (Database db, int id, PatchMovieRequest moviePatchRequest) => {
        var movie = await db.Movies.FindAsync(id);

        if (movie.Deleted) TypedResults.StatusCode(StatusCodes.Status410Gone);

        // Likely a better way to do this all on server (SQL) side.
        movie.Title = moviePatchRequest.Title ?? movie.Title;
        movie.Description = moviePatchRequest.Description ?? movie.Description;
        movie.Duration = moviePatchRequest.Duration ?? movie.Duration;
        movie.Genre = moviePatchRequest.Genre ?? movie.Genre;
        await db.SaveChangesAsync();
        return TypedResults.Ok(movie);
    })
    .WithDescription("Update an existing movie by id");


app.MapDelete("/movie/{id:int}", async (Database db, int id) => {
        var movie = await db.Movies.FindAsync(id);
        movie.Deleted = true;
        await db.SaveChangesAsync();
        return TypedResults.Ok();
    })
    .WithDescription("Delete a movie by id");


app.Run();


public record AddMovieRequest(string Title, string Description, string Duration, string Genre);

public record PatchMovieRequest(string? Title, string? Description, string? Duration, string? Genre);

public class Movie(int? id, string title, string description, string duration, string genre) {
    public int? Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public string Duration { get; set; } = duration;
    public string Genre { get; set; } = genre;

    [JsonIgnore] public bool Deleted { get; set; } = false;
};

class Database : DbContext {
    public Database(DbContextOptions<Database> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
}

// This is a workaround to allow tests to target entry point.
//   honestly might be better to make this a normal class entrypoint.
public partial class Program {}

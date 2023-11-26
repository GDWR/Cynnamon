using Microsoft.EntityFrameworkCore;

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

app.MapGet("/movie/{id}", async (Database db, int id) => TypedResults.Ok(await db.Movies.FindAsync(id)))
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


app.Run();


public record AddMovieRequest(string Title, string Description, string Duration, string Genre);
public record Movie(int? Id, string Title, string Description, string Duration, string Genre);

class Database : DbContext {
    public Database(DbContextOptions<Database> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
}

// This is a workaround to allow tests to target entry point.
//   honestly might be better to make this a normal class entrypoint.
# if DEBUG
public partial class Program {
}
#endif

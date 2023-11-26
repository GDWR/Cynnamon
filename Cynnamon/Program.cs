using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

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

app.MapGet("/movie", (Database db) => db.Movies.ToListAsync())
    .WithOpenApi()
    .WithDescription("Get all movies")
    .Produces<IEnumerable<Movie>>(StatusCodes.Status200OK);

app.MapPost("/movie", (AddMovieRequest movie) => {
        var urlEncodedTitle = UrlEncoder.Default.Encode(movie.Title);
        return Results.Created($"/movie/{urlEncodedTitle}", movie);
    })
    .WithOpenApi()
    .WithDescription("Add a new movie")
    .Produces(StatusCodes.Status201Created);


app.Run();


public record AddMovieRequest(string Title, string Description, string Duration, string Genre);
public record Movie(int Id, string Title, string Description, string Duration, string Genre);

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

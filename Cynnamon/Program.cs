var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/movie", () => {
        return new[] {
            new Movie("The Matrix", "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.", "2h 16m", "Action, Sci-Fi"),
            new Movie("The Matrix Reloaded", "Freedom fighters Neo, Trinity and Morpheus continue to lead the revolt against the Machine Army, unleashing their arsenal of extraordinary skills and weaponry against the systematic forces of repression and exploitation.", "2h 18m", "Action, Sci-Fi"),
            new Movie("The Matrix Revolutions", "The human city of Zion defends itself against the massive invasion of the machines as Neo fights to end the war at another front while also opposing the rogue Agent Smith.", "2h 9m", "Action, Sci-Fi"),
        };
    })
    .WithOpenApi()
    .WithDescription("Get all movies")
    .Produces<IEnumerable<Movie>>(StatusCodes.Status200OK);

app.Run();


public record Movie(string Title, string Description, string Duration, string Genre);

// This is a workaround to allow tests to target entry point.
//   honestly might be better to make this a normal class entrypoint.
# if DEBUG
public partial class Program {
}
#endif

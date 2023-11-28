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

using Cynnamon.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("Cynnamon"))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// This is a workaround to allow tests to target entry point.
//   honestly might be better to make this a normal class entrypoint.
public partial class Program {
}

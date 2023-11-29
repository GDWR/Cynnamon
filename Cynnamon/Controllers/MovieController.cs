using Cynnamon.Models;
using Cynnamon.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cynnamon.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController(DatabaseContext db) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<List<Movie>>> Get() => Ok(await db.Movies.ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Movie>> GetById(int id) => await db.Movies.FindAsync(id) switch {
        null => NotFound(),
        { Deleted: true } => StatusCode(StatusCodes.Status410Gone),
        var movie => Ok(movie),
    };
    
    [HttpPost]
    public ActionResult<Movie> Add(AddMovieRequest movieRequest) {
        var movie = new Movie(
            null,
            movieRequest.Title,
            movieRequest.Description,
            movieRequest.Duration,
            movieRequest.Genre
        );

        db.Add(movie);
        db.SaveChanges();

        return Created($"/movie/{movie.Id}", movie);
    }
    
    [HttpPatch("{id:int}")]
    public ActionResult<Movie> Update(int id,  UpdateMovieRequest movieUpdateRequest) {
        var movie = db.Movies.Find(id);

        if (movie is null) return NotFound();
        if (movie.Deleted) return StatusCode(StatusCodes.Status410Gone);

        // Likely a better way to do this all on server (SQL) side.
        movie.Title = movieUpdateRequest.Title ?? movie.Title;
        movie.Description = movieUpdateRequest.Description ?? movie.Description;
        movie.Duration = movieUpdateRequest.Duration ?? movie.Duration;
        movie.Genre = movieUpdateRequest.Genre ?? movie.Genre;
        db.SaveChanges();
        return Ok(movie);
    }
    
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id) {
        var movie = db.Movies.Find(id);

        if (movie is null) return NotFound();
        if (movie.Deleted) return StatusCode(StatusCodes.Status410Gone);

        movie.Deleted = true;
        db.SaveChanges();
        return Ok();
    }
}

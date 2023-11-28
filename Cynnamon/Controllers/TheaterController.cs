using Cynnamon.Database;
using Cynnamon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cynnamon.Controllers;

[ApiController]
[Route("[controller]")]
public class TheaterController(DatabaseContext db) : ControllerBase {

    [HttpPost]
    public ActionResult<Theater> Add(AddTheaterRequest theaterRequest)
    {
        var theater = new Theater(
            null,
            theaterRequest.Name,
            theaterRequest.Location,
            theaterRequest.Seats);

        db.Add(theater);
        db.SaveChanges();

        return Created($"/theater/{theater.Id}", theater);
    }
    
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Theater>> Update(UpdateTheaterRequest theaterRequest, int id)
    {
        var theater = await db.Theaters.FindAsync(id);
        
        if (theater == null) return NotFound();
        
        theater.Name = theaterRequest.Name ?? theater.Name;
        theater.Location = theaterRequest.Location ?? theater.Location;
        theater.Seats = theaterRequest.Seats ?? theater.Seats;

        await db.SaveChangesAsync();

        return Ok(theater);
    }
}

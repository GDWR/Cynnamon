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
}

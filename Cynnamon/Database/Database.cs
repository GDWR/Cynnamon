using Cynnamon.Models;
using Microsoft.EntityFrameworkCore;

namespace Cynnamon.Database;

public class DatabaseContext : DbContext {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
}

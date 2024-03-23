using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Models;

public class CovidAppDbContext:DbContext
{
    public CovidAppDbContext(DbContextOptions<CovidAppDbContext> options):base(options)
    {
        
    }
    public DbSet<Covid> Covids { get; set; }
}

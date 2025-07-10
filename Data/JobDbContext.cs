using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class JobDbContext : DbContext
{
    public DbSet<JobOffer> JobOffers => Set<JobOffer>();
    
    public JobDbContext() { }

    public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
            options.UseSqlite("Data Source=jobs.db");
    }
}

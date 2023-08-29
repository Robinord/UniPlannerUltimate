using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;
using UniPlanner.Models;

namespace UniPlanner.Areas.Identity.Data;

public class UniPlannerContext : IdentityDbContext<UniPlannerUser>
{
    public UniPlannerContext(DbContextOptions<UniPlannerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<UniPlanner.Models.UniversityInfo> UniversityInfo { get; set; } = default!;

    public DbSet<UniPlanner.Models.Programme> Programme { get; set; } = default!;

    public DbSet<UniPlanner.Models.UniProgramme> UniProgramme { get; set; } = default!;

    public DbSet<UniPlanner.Models.MajorsOffered> MajorsOffered { get; set; } = default!;
}

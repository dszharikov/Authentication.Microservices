using Microsoft.EntityFrameworkCore;

namespace Password;

public class ApplicationDbContext : DbContext
{
    public DbSet<OneTimePassword> OneTimePasswords { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
using Microsoft.EntityFrameworkCore;

namespace PasswordGeneratorAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Password> Passwords { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}

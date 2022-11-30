using Contacts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
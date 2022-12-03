using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;
}
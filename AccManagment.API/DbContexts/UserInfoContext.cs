using AccManagment.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccManagment.API.DbContecsts;


public class UserInfoContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<User> Users { get; set; }

    public UserInfoContext(DbContextOptions<UserInfoContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _ = optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:UserInfoDb"]);
        }
    }
}


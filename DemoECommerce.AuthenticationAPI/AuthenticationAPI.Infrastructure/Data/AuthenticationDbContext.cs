using Microsoft.EntityFrameworkCore;
using AuthenticationAPI.Domain.Entities;

namespace AuthenticationAPI.Infrastructure.Data
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}

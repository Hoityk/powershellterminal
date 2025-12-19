using Microsoft.EntityFrameworkCore;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Theme> Themes { get; set; } // <--- ДОДАЙ ЦЕЙ РЯДОК

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=terminal.db");
    }
}
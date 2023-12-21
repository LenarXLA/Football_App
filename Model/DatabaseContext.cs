using Microsoft.EntityFrameworkCore;
using System;

namespace Football_App.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}Footbal.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FanClub>().HasKey(key => new { key.ClubId, key.FanId });
        }

        
        public DbSet<FootballPlayer> FootballPlayers { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Fan> Fans { get; set; }
        public DbSet<FanClub> FanClubs { get; set; }

    }
}

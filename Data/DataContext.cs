using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Battleship_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Player1Coordinate> Player1Coordinates { get; set; }
        public DbSet<Player2Coordinate> Player2Coordinates { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Player1Coordinate>().HasKey(x => x.CoordinateId);
            builder.Entity<Player1Coordinate>().Property(x => x.CoordinateId).IsRequired();
            builder.Entity<Player1Coordinate>().Property(x => x.IsHit).HasDefaultValue(false);
            builder.Entity<Player1Coordinate>().Property(x => x.IsShip).HasDefaultValue(false);

            builder.Entity<Player2Coordinate>().HasKey(x => x.CoordinateId);
            builder.Entity<Player2Coordinate>().Property(x => x.CoordinateId).IsRequired();
            builder.Entity<Player2Coordinate>().Property(x => x.IsHit).HasDefaultValue(false);
            builder.Entity<Player2Coordinate>().Property(x => x.IsShip).HasDefaultValue(false);


            base.OnModelCreating(builder);
        }
    }

}

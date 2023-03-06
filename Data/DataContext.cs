using Battleship_API.Data.Mappings;
using Microsoft.EntityFrameworkCore;

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
            builder.ApplyConfiguration(new Player1CoordinatesMapping());
            builder.ApplyConfiguration(new Player2CoordinatesMapping());

            base.OnModelCreating(builder);
        }
    }

}

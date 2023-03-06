using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Battleship_API.Data.Mappings
{
    public abstract class PlayerCoordinatesMapping<TBase> : IEntityTypeConfiguration<TBase>
        where TBase : PlayerCoordinate
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.HasKey(x => x.CoordinateId);
            builder.Property(x => x.CoordinateId).IsRequired();
            builder.Property(x => x.IsHit).HasDefaultValue(false);
            builder.Property(x => x.IsShip).HasDefaultValue(false);
        }
    }
    public class Player1CoordinatesMapping : PlayerCoordinatesMapping<Player1Coordinate>
    {
        public override void Configure(EntityTypeBuilder<Player1Coordinate> builder)
        {
            base.Configure(builder);
        }
    }
    public class Player2CoordinatesMapping : PlayerCoordinatesMapping<Player2Coordinate>
    {
        public override void Configure(EntityTypeBuilder<Player2Coordinate> builder)
        {
            base.Configure(builder);
        }
    }
}

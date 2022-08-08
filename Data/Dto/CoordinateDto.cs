namespace Battleship_API.Data.Dto
{
    public class CoordinateDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsShip { get; set; } = false;
        public bool IsHit { get; set; } = false;
    }
}

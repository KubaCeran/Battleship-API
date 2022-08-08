namespace Battleship_API.Data
{
    public class Player1Coordinate
    {
        public int CoordinateId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsShip { get; set; }
        public bool IsHit { get; set; }
    }
}

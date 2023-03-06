namespace Battleship_API.Data
{
    public abstract class PlayerCoordinate
    {
        public int CoordinateId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsShip { get; set; }
        public bool IsHit { get; set; }
    }

    public class Player1Coordinate : PlayerCoordinate
    {

    }
    public class Player2Coordinate : PlayerCoordinate
    {

    }
}

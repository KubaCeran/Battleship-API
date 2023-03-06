namespace Battleship_API.Data.Models
{
    public class Board
    {
        public int Width { get; set; } = 4;
        public int Length { get; set; } = 4;
        public int StartIndex { get; }
        public int EndIndexWidth { get; }
        public int EndIndexLength { get; }
        public Board()
        {
            this.StartIndex = 1;
            this.EndIndexWidth = this.Width + 1;
            this.EndIndexLength = this.Length + 1;
        }
    }
}

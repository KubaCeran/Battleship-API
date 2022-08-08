namespace Battleship_API.Data.Dto
{
    public class ResponseDto
    {
        public List<CoordinateDto> Coordinates { get; set; }
        public bool IsP1Winner { get; set; } = false;
        public bool IsP2Winner { get; set; } = false;
        public int Player { get; set; }
    }
}

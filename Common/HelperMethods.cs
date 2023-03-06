using Battleship_API.Data.Dto;
using Battleship_API.Data.Models;

namespace Battleship_API.Common
{
    public static class HelperMethods
    {
        public static bool CheckPlayer(int playerId)
        {
            if (playerId == 1 || playerId == 2)
            {
                return true;
            }
            return false;
        }
        public static List<CoordinateDto> PlaceShips()
        {
            var rand = new Random();
            var takenCoordinatesDtoList = new List<CoordinateDto>();
            var board = new Board 
            { 
                Width = 10, 
                Length = 10
            };

            foreach (var ship in Enum.GetValues(typeof(Ship)))
            {
                bool isSearching = true;
                while (isSearching)
                {
                    var startcolumn = rand.Next(board.StartIndex, board.EndIndexWidth);
                    var startrow = rand.Next(board.StartIndex, board.EndIndexLength);
                    var orientation = rand.Next(0, 2); //0 for Horizontal

                    var tempList = GenerateCoordinates((int)ship, startcolumn, startrow, orientation);

                    //CHECKING IF COORDINATES ARE WITHIN GIVEN BOUNDRIES
                    var checkRow = tempList.FirstOrDefault(x => x.X > board.Length);
                    var checkColumn = tempList.FirstOrDefault(x => x.Y > board.Width);

                    if (checkRow != null || checkColumn != null)
                    {
                        tempList.Clear();
                        isSearching = true;
                        continue;
                    }

                    //CHECKING IF COORDINATE IS TAKEN
                    bool containsCommonItem = CoordinateTaken(takenCoordinatesDtoList, tempList);

                    if (containsCommonItem)
                    {
                        tempList.Clear();
                        isSearching = true;
                        continue;
                    }
                    takenCoordinatesDtoList.AddRange(tempList);
                    tempList.Clear();
                    isSearching = false;
                }
            }
            return takenCoordinatesDtoList;
        }
        public static List<CoordinateDto> CreateEmptyBoard()
        {
            var board = new Board
            {
                Width = 10,
                Length = 10
            };
            var allCoordinateDtoList = new List<CoordinateDto>();

            for (int i = 1; i <= board.Width; i++)
            {
                for (int j = 1; j <= board.Length; j++)
                {
                    var coordinate = new CoordinateDto
                    {
                        X = i,
                        Y = j,
                    };
                    allCoordinateDtoList.Add(coordinate);
                }
            }
            return allCoordinateDtoList;
        }

        public static bool CoordinateTaken(List<CoordinateDto> allCoordinates, List<CoordinateDto> coordinatesToCheck)
        {
            var isCommon = false;
            if (allCoordinates.Count == 0)
            {
                isCommon = false;
            }
            else
            {
                foreach (var cor in coordinatesToCheck)
                {
                    if (allCoordinates.Any(x => x.X == cor.X && x.Y == cor.Y))
                    {
                        isCommon = true;
                        break;
                    }
                    isCommon = false;
                }
            }
            return isCommon;
        }

        public static List<CoordinateDto> GenerateCoordinates(int shipLength, int startColumn, int startRow, int orientation)
        {
            var endcolumn = startColumn;
            var endrow = startRow;
            var tempList = new List<CoordinateDto>();
            tempList.Add(new CoordinateDto { X = startColumn, Y = startRow, IsShip = true });


            if (orientation == 0)
            {
                for (int i = 1; i < shipLength; i++)
                {
                    endrow++;
                    tempList.Add(new CoordinateDto { X = startColumn, Y = endrow, IsShip = true });
                }
            }
            else
            {
                for (int i = 1; i < shipLength; i++)
                {
                    endcolumn++;
                    tempList.Add(new CoordinateDto { X = endcolumn, Y = startRow, IsShip = true });
                }
            }
            return tempList;
        }
    }
}

using Battleship_API.Data.Dto;

namespace Battleship_API.Helpers
{
    public static class HelperMethods
    {
        public static List<CoordinateDto> PlaceShips()
        {
            
                var shipsLength = new List<int> { 2, 3, 4, 5 }; //hardcoded length of each ship according to the rules
                var rand = new Random();
                var takenCoordinatesDtoList = new List<CoordinateDto>();

                foreach (var ship in shipsLength)
                {
                    bool isSearching = true;
                    while (isSearching)
                    {
                        var startcolumn = rand.Next(1, 11);
                        var startrow = rand.Next(1, 11);
                        var orientation = rand.Next(0, 2); //0 for Horizontal

                        var tempList = HelperMethods.GenerateCoordinates(ship,startcolumn, startrow, orientation);

                        //CHECKING IF COORDINATES ARE WITHIN GIVEN BOUNDRIES
                        var checkRow = tempList.FirstOrDefault(x => x.X > 10);
                        var checkColumn = tempList.FirstOrDefault(x => x.Y > 10);

                        if (checkRow != null || checkColumn != null)
                        {
                            tempList.Clear();
                            isSearching = true;
                            continue;
                        }

                        //CHECKING IF COORDINATE IS TAKEN
                        bool containsCommonItem = HelperMethods.CoordinateTaken(takenCoordinatesDtoList, tempList);

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
            var allCoordinateDtoList = new List<CoordinateDto>();

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
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
            if(allCoordinates.Count == 0)
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


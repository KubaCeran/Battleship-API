using Battleship_API.Data.Dto;

namespace Battleship_API.Helpers
{
    public static class HelperMethods
    {
        public static List<CoordinateDto> PlaceShips()
        {
            
                var shipsLength = new List<int> { 2, 3, 3, 4, 5 }; //hardcoded length of each ship according to the rules
                var rand = new Random();
                var takenCoordinatesDtoList = new List<CoordinateDto>();

                foreach (var ship in shipsLength)
                {
                    bool isOpen = true;
                    while (isOpen)
                    {
                        var startcolumn = rand.Next(1, 11);
                        var startrow = rand.Next(1, 11);
                        var endcolumn = startcolumn;
                        var endrow = startrow;

                        var tempList = new List<CoordinateDto>();

                        tempList.Add(new CoordinateDto { X = startcolumn, Y = startrow, IsShip = true });

                        var orientation = rand.Next(1, 101) % 2; //0 for Horizontal

                        if (orientation == 0)
                        {
                            for (int i = 1; i < ship; i++)
                            {
                                endrow++;
                                tempList.Add(new CoordinateDto { X = startcolumn, Y = endrow, IsShip = true });
                            }
                        }
                        else
                        {
                            for (int i = 1; i < ship; i++)
                            {
                                endcolumn++;
                                tempList.Add(new CoordinateDto { X = endcolumn, Y = startrow, IsShip = true });
                            }
                        }

                        //CHECKING IF COORDINATES ARE WITHIN GIVEN BOUNDRIES

                        if (endrow > 10 || endcolumn > 10)
                        {
                            tempList.Clear();
                            isOpen = true;
                            continue;
                        }

                        //CHECKING IF COORDINATE IS TAKEN

                        if (takenCoordinatesDtoList.Count() == 0)
                        {
                            takenCoordinatesDtoList.AddRange(tempList);
                        }
                        else
                        {
                            bool containsCommonItem = false;
                            foreach (var temp in tempList)
                            {
                                if (takenCoordinatesDtoList.Any(x => x.X == temp.X && x.Y == temp.Y))
                                {
                                    containsCommonItem = true;
                                    break;
                                }
                            }
                            if (containsCommonItem)
                            {
                                tempList.Clear();
                                isOpen = true;
                                continue;
                            }
                            takenCoordinatesDtoList.AddRange(tempList);
                            tempList.Clear();
                        }
                        isOpen = false;
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
    }
}


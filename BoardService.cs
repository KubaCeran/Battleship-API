using Battleship_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Battleship_API
{
    public interface IBoardService
    {
        ResponseDto PlaceShips(int id);
        void AddToDatabase(ResponseDto board, int id);
        void SaveChanges();
        void Clear();
    }
    public class BoardService : IBoardService
    {
        private readonly DataContext _context;

        public BoardService(DataContext context)
        {
            _context = context;
        }
        public ResponseDto PlaceShips(int id)
        {
            //LOGIC FOR CREATING AN EMPTY BOARD
            var allCoordinateDtoList = new List<CoordinateDto>();
            
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    var coordinate = new CoordinateDto
                    {
                        X = i,
                        Y = j,
                    };
                    allCoordinateDtoList.Add(coordinate);
                }
            }

            //LOGIC FOR PLACING THE SHIPS

            var shipsLength = new List<int> { 2, 2, 3 }; //hardcoded length of each ship
            var rand = new Random();
            var takenCoordinatesDtoList = new List<CoordinateDto>();


            foreach (var ship in shipsLength)
            {
                bool isOpen = true;
                while (isOpen)
                {
                    var startcolumn = rand.Next(1, 4);
                    var startrow = rand.Next(1, 4);
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

                    if (endrow > 4 || endcolumn > 4)
                    {
                        tempList.Clear();
                        isOpen = true;
                        continue;
                    }

                    //CHECKING IF COORDINATE IS TAKEN

                    if(takenCoordinatesDtoList.Count() == 0)
                    {
                        takenCoordinatesDtoList.AddRange(tempList);
                    }
                    else
                    {
                        bool containsCommonItem = false;
                        foreach(var temp in tempList)
                        {
                            if(takenCoordinatesDtoList.Any(x => x.X == temp.X && x.Y == temp.Y))
                            {
                                containsCommonItem = true;
                                break;
                            }
                        }
                        if(containsCommonItem)
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

            var finalList = allCoordinateDtoList.Where(x => !takenCoordinatesDtoList.Any(y => y.X == x.X && y.Y == x.Y)).ToList();
            finalList.AddRange(takenCoordinatesDtoList);
            allCoordinateDtoList = finalList.OrderBy(x => x.X).ThenBy(y => y.Y).ToList();

            var reponseDto = new ResponseDto
            {
                Coordinates = allCoordinateDtoList,
                IsP1Winner = false,
                IsP2Winner =false,
                Player = id
            };
            return reponseDto;
        }

        public void AddToDatabase(ResponseDto board, int id)
        {
            var coordinateDtoList = board.Coordinates;

            if(id == 1)
            {
                foreach (var cor in coordinateDtoList)
                {
                    var coordinateP1 = new Player1Coordinate
                    {
                        X = cor.X,
                        Y = cor.Y,
                        IsHit = cor.IsHit,
                        IsShip = cor.IsShip
                    };
                    _context.Player1Coordinates.Add(coordinateP1);
                }
            }
            else
            {
                foreach (var cor in coordinateDtoList)
                {
                    var coordinateP2 = new Player2Coordinate
                    {
                        X = cor.X,
                        Y = cor.Y,
                        IsHit = cor.IsHit,
                        IsShip = cor.IsShip
                    };
                    _context.Player2Coordinates.Add(coordinateP2);
                }
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Clear()
        {
            var rowsP1 = _context.Player1Coordinates.Select(x => x).ToList();
            var rowsP2 = _context.Player2Coordinates.Select(x => x).ToList();

            foreach (var row in rowsP1)
            {
                _context.Player1Coordinates.Remove(row);
            }

            foreach (var row in rowsP2)
            {
                _context.Player2Coordinates.Remove(row);
            }
        }
    }

}

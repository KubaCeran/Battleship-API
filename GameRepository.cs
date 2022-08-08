using Battleship_API.Data;
using Battleship_API.Data.Dto;

namespace Battleship_API
{
    public interface IGameRepository
    {
        List<CoordinateDto> GetBoardForPlayer(int playerId);
        void GenerateHit(int playerId);
        void AddToDatabase(ResponseDto responseDto, int playerId);
        void SaveChanges();
        void ClearDatabase();
    }
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }
        

        public void AddToDatabase(ResponseDto responseDto, int playerId)
        {
            var coordinateDtoList = responseDto.Coordinates;

            if (playerId == 1)
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
            if(playerId == 2)
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

        public void ClearDatabase()
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

        public void GenerateHit(int playerId)
        {
            var rand = new Random();

            if (playerId == 1)
            {
                var availableCorsList = _context.Player2Coordinates.Where(x => x.IsHit == false).ToList();
                var radomCorIndex = rand.Next(availableCorsList.Count);
                var randomCor = availableCorsList[radomCorIndex];
                var shotCor = _context.Player2Coordinates.Where(x => x == randomCor).SingleOrDefault();
                shotCor.IsHit = true;
            }
            if (playerId == 2)
            {
                var availableCorsList = _context.Player1Coordinates.Where(x => x.IsHit == false).ToList();
                var radomCorIndex = rand.Next(availableCorsList.Count);
                var randomCor = availableCorsList[radomCorIndex];
                var shotCor = _context.Player1Coordinates.Where(x => x == randomCor).SingleOrDefault();
                shotCor.IsHit = true;
            }
        }

        public List<CoordinateDto> GetBoardForPlayer(int playerId)
        {
            var board = new List<CoordinateDto>();

            if (playerId == 1)
            {
                var data = _context.Player2Coordinates.ToList();
                foreach (var row in data)
                {
                    board.Add(new CoordinateDto
                    {
                        X = row.X,
                        Y = row.Y,
                        IsHit = row.IsHit,
                        IsShip = row.IsShip,
                    });
                }
            }
            if (playerId == 2)
            {
                var data = _context.Player1Coordinates.ToList();
                foreach (var row in data)
                {
                    board.Add(new CoordinateDto
                    {
                        X = row.X,
                        Y = row.Y,
                        IsHit = row.IsHit,
                        IsShip = row.IsShip,
                    });
                }
            }
            return board;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

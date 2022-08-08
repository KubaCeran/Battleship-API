using Battleship_API.Data;
using Battleship_API.Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace Battleship_API
{
    public interface IGameRepository
    {
        Task<List<CoordinateDto>> GetBoardForPlayer(int playerId);
        Task GenerateHit(int playerId);
        Task AddToDatabase(ResponseDto responseDto, int playerId);
        Task<bool> SaveChanges();
        Task<bool> ClearDatabase();
    }
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }
        

        public async Task AddToDatabase(ResponseDto responseDto, int playerId)
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
                    await _context.Player1Coordinates.AddAsync(coordinateP1);
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
                    await _context.Player2Coordinates.AddAsync(coordinateP2);
                }
            }
        }

        public async Task<bool> ClearDatabase()
        {
            var rowsP1 = await _context.Player1Coordinates.Select(x => x).ToListAsync();
            var rowsP2 = await _context.Player2Coordinates.Select(x => x).ToListAsync();

            foreach (var row in rowsP1)
            {
                _context.Player1Coordinates.Remove(row);
            }

            foreach (var row in rowsP2)
            {
                _context.Player2Coordinates.Remove(row);
            }
            return true;
        }

        public async Task GenerateHit(int playerId)
        {
            var rand = new Random();

            if (playerId == 1)
            {
                var allCors = await _context.Player2Coordinates.ToListAsync();
                var availableCorsList = allCors.Where(x => x.IsHit == false).ToList();
                var radomCorIndex = rand.Next(availableCorsList.Count);
                var randomCor = availableCorsList[radomCorIndex];
                var shotCor = _context.Player2Coordinates.Where(x => x == randomCor).SingleOrDefault();
                shotCor.IsHit = true;
            }
            if (playerId == 2)
            {
                var allCors = await _context.Player1Coordinates.ToListAsync();
                var availableCorsList = allCors.Where(x => x.IsHit == false).ToList();
                var radomCorIndex = rand.Next(availableCorsList.Count);
                var randomCor = availableCorsList[radomCorIndex];
                var shotCor = _context.Player1Coordinates.Where(x => x == randomCor).SingleOrDefault();
                shotCor.IsHit = true;
            }
        }

        public async Task<List<CoordinateDto>> GetBoardForPlayer(int playerId)
        {
            var board = new List<CoordinateDto>();

            if (playerId == 1)
            {
                var data = await _context.Player2Coordinates.ToListAsync();
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
                var data = await _context.Player1Coordinates.ToListAsync();
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

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

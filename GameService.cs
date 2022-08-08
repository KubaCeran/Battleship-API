using Battleship_API.Data.Dto;
using Battleship_API.Helpers;

namespace Battleship_API
{
    public interface IGameService
    {
        Task<ResponseDto> GenerateBoardWithShipsForPlayer(int playerId);
        Task<ResponseDto> Move(int playerId);
        Task<bool> Clear();
    }
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<ResponseDto> GenerateBoardWithShipsForPlayer(int playerId)
        {
            var emptyCoordinatesList = HelperMethods.CreateEmptyBoard();
            var shipsCoordinatesList = HelperMethods.PlaceShips();

            var tempCoordinates = emptyCoordinatesList.Where(x => !shipsCoordinatesList.Any(y => y.X == x.X && y.Y == x.Y)).ToList();
            tempCoordinates.AddRange(shipsCoordinatesList);
            var coordinatesToDatabase = tempCoordinates.OrderBy(x => x.Y).ThenBy(y => y.X).ToList();

            var reponseDto = new ResponseDto
            {
                Coordinates = coordinatesToDatabase,
                IsP1Winner = false,
                IsP2Winner =false,
                Player = playerId
            };
            await _gameRepository.AddToDatabase(reponseDto, playerId);
            await _gameRepository.SaveChanges();
            return reponseDto;
        }
        public async Task<bool> Clear()
        {
            if(await _gameRepository.ClearDatabase() && await _gameRepository.SaveChanges())
            {
                return true;
            }
            return false;
            
        }

        public async Task<ResponseDto> Move(int playerId)
        {
            await _gameRepository.GenerateHit(playerId);
            await _gameRepository.SaveChanges();

            var playerBoard = await _gameRepository.GetBoardForPlayer(playerId);
            var isAllDown = playerBoard.Where(x => x.IsShip == true).All(y => y.IsHit == true);
            
            var response = new ResponseDto
            {
                Coordinates = playerBoard
            };
            if (playerId == 1)
            {
                response.IsP1Winner = isAllDown;
                response.Player = 2;
            }
            if(playerId == 2)
            {
                response.IsP2Winner = isAllDown;
                response.Player = 1;
            }
            return response;
        }
    }
}

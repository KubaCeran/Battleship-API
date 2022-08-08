using Battleship_API.Data.Dto;
using Battleship_API.Helpers;

namespace Battleship_API
{
    public interface IGameService
    {
        ResponseDto GenerateBoardWithShipsForPlayer(int playerId);
        ResponseDto Move(int playerId);
        void Clear();
    }
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public ResponseDto GenerateBoardWithShipsForPlayer(int playerId)
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
            _gameRepository.AddToDatabase(reponseDto, playerId);
            _gameRepository.SaveChanges();
            return reponseDto;
        }
        public void Clear()
        {
            _gameRepository.ClearDatabase();
            _gameRepository.SaveChanges();
        }

        public ResponseDto Move(int playerId)
        {
            _gameRepository.GenerateHit(playerId);
            _gameRepository.SaveChanges();

            var playerBoard = _gameRepository.GetBoardForPlayer(playerId);
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

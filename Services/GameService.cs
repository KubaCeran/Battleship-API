using Battleship_API.Common;
using Battleship_API.Data.Dto;
using Battleship_API.Services;

namespace Battleship_API
{
    public interface IGameService
    {
        Task<ServiceResult<ResponseDto>> GenerateBoardWithShipsForPlayer(int playerId);
        Task<ServiceResult<ResponseDto>> Move(int playerId);
        Task<ServiceResult> Clear();
    }
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<ServiceResult<ResponseDto>> GenerateBoardWithShipsForPlayer(int playerId)
        {
            if(!HelperMethods.CheckPlayer(playerId))
            {
                return ServiceResult<ResponseDto>.WithErrors($"No player with given ID of {playerId}");
            }

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

            try
            {
                await _gameRepository.AddToDatabase(reponseDto, playerId);
            }
            catch (Exception ex)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Error during clearing the boards, {ex}");
            }

            try
            {
                await _gameRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Error during saving, {ex}");
            }
            return ServiceResult<ResponseDto>.WithSuccess(reponseDto);
        }
        public async Task<ServiceResult> Clear()
        {
            try
            {
                await _gameRepository.ClearDatabase();
            }
            catch (Exception ex)
            {
                return ServiceResult.WithErrors($"Error during clearing the boards, {ex}");
            }

            try
            {
                await _gameRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                return ServiceResult.WithErrors($"Error during saving, {ex}");
            }
            return ServiceResult.WithSuccess();
        }

        public async Task<ServiceResult<ResponseDto>> Move(int playerId)
        {
            if (!HelperMethods.CheckPlayer(playerId))
            {
                return ServiceResult<ResponseDto>.WithErrors($"No player with given ID of {playerId}");
            }

            try
            {
                await _gameRepository.GenerateHit(playerId);
            }
            catch(Exception ex)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Error during generating move, {ex}");
            }

            try
            {
                await _gameRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Error during saving, {ex}");
            }

            try
            {
                await _gameRepository.GenerateHit(playerId);
            }
            catch (Exception ex)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Error during generating move, {ex}");
            }

            var playerBoard = await _gameRepository.GetBoardForPlayer(playerId);
            if (playerBoard == null)
            {
                return ServiceResult<ResponseDto>.WithErrors($"Cannot get the board for a player {playerId}");
            }
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
            return ServiceResult<ResponseDto>.WithSuccess(response);
        }
    }
}

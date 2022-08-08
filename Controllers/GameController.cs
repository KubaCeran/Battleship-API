using Battleship_API.Data.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleship_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

       [HttpGet("{playerId}")]
        public ResponseDto GetBoards(int playerId)
        {
            return _gameService.GenerateBoardWithShipsForPlayer(playerId);
        }

        [HttpGet("move/{playerId}")]
        public ResponseDto Move(int playerId)
        {
            return _gameService.Move(playerId);
        }

        [HttpGet("reset")]
        public void ResetBoards()
        {
            _gameService.Clear();
        }
    }
}

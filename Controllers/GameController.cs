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
        public async Task<ActionResult<ResponseDto>> GetBoards(int playerId)
        {
            return await _gameService.GenerateBoardWithShipsForPlayer(playerId);
        }

        [HttpGet("move/{playerId}")]
        public async Task<ActionResult<ResponseDto>> Move(int playerId)
        {
            return await _gameService.Move(playerId);
        }

        [HttpGet("reset")]
        public async Task<ActionResult> ResetBoards()
        {
            if (await _gameService.Clear())
            {
                return Ok();
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

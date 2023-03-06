using Battleship_API.Data.Dto;
using Battleship_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleship_API.Controllers
{
    
    public class GameController : BaseApiController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<ServiceResult<ResponseDto>>> GetBoards(int playerId)
        {
            var result =  await _gameService.GenerateBoardWithShipsForPlayer(playerId);
            if (result.IsError == true)
                return BadRequest(result.ErrorsMessage);
            return Ok(result.Result);
        }

        [HttpGet("move/{playerId}")]
        public async Task<ActionResult<ServiceResult<ResponseDto>>> Move(int playerId)
        {
            var result = await _gameService.Move(playerId);
            if (result.IsError == true)
                return BadRequest(result.ErrorsMessage);
            return Ok(result.Result);
        }

        [HttpGet("reset")]
        public async Task<ActionResult<ServiceResult>> ResetBoards()
        {
            var result = await _gameService.Clear();
            if (result.IsError == true)
                return BadRequest(result.ErrorsMessage);
            return Ok("Success");
        }
    }
}

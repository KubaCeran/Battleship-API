using Battleship_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleship_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public GameController(IBoardService boardService)
        {
            _boardService = boardService;
        }

       [HttpGet("{id}")]
        public ResponseDto GetBoards(int id)
        {
            var responseDto = _boardService.PlaceShips(id);
            _boardService.AddToDatabase(responseDto, id);
            _boardService.SaveChanges();
            return responseDto;
        }

        [HttpGet("reset")]
        public void ResetBoards()
        {
            _boardService.Clear();
            _boardService.SaveChanges();
        }
    }
}

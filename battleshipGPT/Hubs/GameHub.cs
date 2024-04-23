using battleshipGPT.Serives;
using battleshipGPT.Services;
using Microsoft.AspNetCore.SignalR;

namespace battleshipGPT.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;
        private readonly RoomService _roomService;
        public GameHub(GameService gameService, RoomService roomService)
        {
            _gameService = gameService;
            _roomService = roomService;
        }
        public async Task PlayerMove(string roomId, int selectedRow, int selectedCol)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            _gameService.SetPoint(currentRoom, selectedRow, selectedCol);


        }

        public async Task EnemyMove(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            _gameService.EnemySetPoint(currentRoom);
        }
    }
}

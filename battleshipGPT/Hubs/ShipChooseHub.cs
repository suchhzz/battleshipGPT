using battleshipGPT.Models;
using battleshipGPT.Serives;
using battleshipGPT.Services;
using Microsoft.AspNetCore.SignalR;

namespace battleshipGPT.Hubs
{
    public class ShipChooseHub : Hub
    {
        private readonly GameService _gameService;
        private readonly RoomService _roomService;
        public ShipChooseHub(RoomService roomService, GameService gameService)
        {
            _roomService = roomService;
            _gameService = gameService;
        }
        public async Task SetUsersParameters()
        {
            Guid userId = Guid.NewGuid();
            Guid roomId = Guid.NewGuid();

            _roomService.CreateRoom(userId, roomId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync("SetParameters", userId, roomId);
        }

        public async Task SetShip(string roomId, int headX, int headY, int deck, bool horizontal)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                _gameService.SetShip(currentRoom, headX, headY, deck, horizontal);
            }
        }

        public async Task CreateEnemyPlayground(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                currentRoom.enemyShips = _gameService.CreateEnemyShips(currentRoom);
            }

            await Clients.Group(roomId).SendAsync("StartGame");
        }
    }
}

using battleshipGPT.Models;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Serives;
using battleshipGPT.Services;
using Microsoft.AspNetCore.SignalR;

namespace battleshipGPT.Hubs
{
    public class ShipChooseHub : Hub
    {
        private readonly ShipChooseService _shipChooseService;
        private readonly RoomService _roomService;
        public ShipChooseHub(RoomService roomService, ShipChooseService shipChooseService)
        {
            _roomService = roomService;
            _shipChooseService = shipChooseService;
        }
        public async Task SetUsersParameters()
        {
            User user = new User();
            Guid roomId = Guid.NewGuid();

            _roomService.CreateRoom(user, roomId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync("SetParameters", user, roomId);
        }

        public async Task SetShip(string roomId, int headX, int headY, int deck, bool horizontal)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                _shipChooseService.SetShip(currentRoom, headX, headY, deck, horizontal);
            }
        }

        public async Task CreateEnemyPlayground(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                currentRoom.enemyShips = _shipChooseService.CreateEnemyShips(currentRoom);
            }

            await Clients.Group(roomId).SendAsync("StartGame");
        }
    }
}

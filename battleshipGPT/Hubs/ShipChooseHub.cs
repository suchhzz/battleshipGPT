using battleshipGPT.Models;
using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Models.PlayersModel;
using battleshipGPT.Serives;
using battleshipGPT.Services;
using Microsoft.AspNetCore.SignalR;

namespace battleshipGPT.Hubs
{
    public class ShipChooseHub : Hub
    {
        private readonly ILogger<ShipChooseHub> _logger;
        private readonly ShipChooseService _shipChooseService;
        private readonly RoomService _roomService;
        private readonly LogService _logService;
        public ShipChooseHub(RoomService roomService, ShipChooseService shipChooseService, ILogger<ShipChooseHub> logger, LogService logService)
        {
            _roomService = roomService;
            _shipChooseService = shipChooseService;
            _logger = logger;
            _logService = logService;
        }
        public async Task SetUsersParameters()
        {
            Player player = new Player
            {
                Id = Guid.NewGuid(),
                PlayerShips = new List<ShipModel>(),
                PlayerShipsRemaining = 0
            };

            Guid roomId = Guid.NewGuid();

            _roomService.CreateRoom(player, roomId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync("SetParameters", player.Id, roomId);
        }

        public async Task SetShip(string roomId, int x, int y, int deck, bool horizontal)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                _shipChooseService.SetShip(currentRoom, x, y, deck, horizontal);
            }

            _logger.LogInformation($"player ships: {currentRoom.Player.PlayerShips.Count}");

            if (currentRoom.Player.PlayerShips.Count >= 10)
            {
                currentRoom.Player.PlayerShipsRemaining = 1;
                await CreateEnemyPlayground(currentRoom.RoomId.ToString());
            }
        }

        public async Task CreateEnemyPlayground(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom != null)
            {
                _logger.LogInformation("generating enemy field..");
                _shipChooseService.CreateEnemyShips(currentRoom);


                _logger.LogInformation("\n\nenemy field:");
                _logService.showField(currentRoom.enemy.EnemyShips, "enemy");


                _logger.LogInformation("\n\nplayer`s field:");
                _logService.showField(currentRoom.Player.PlayerShips, "player");
            }


            await Clients.Group(roomId).SendAsync("StartGame");
        }

        public async Task Test(string roomId)
        {
            _logger.LogInformation("test method has been invoked, roomId: " + roomId);
        }

        
    }
}

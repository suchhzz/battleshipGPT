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
        public ShipChooseHub(RoomService roomService, ShipChooseService shipChooseService, ILogger<ShipChooseHub> logger)
        {
            _roomService = roomService;
            _shipChooseService = shipChooseService;
            _logger = logger;
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

            if (currentRoom.Player.PlayerShipsRemaining >= 0)
            {
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

                fillEnemyPlayground(currentRoom);
            }

            await Clients.Group(roomId).SendAsync("StartGame");
        }

        public async Task Test(string roomId)
        {
            _logger.LogInformation("test method has been invoked, roomId: " + roomId);
        }

        private char[,] enemyPlayground = new char[10, 10];
        private void fillEnemyPlayground(Room room)
        {
            for (int i = 0; i < enemyPlayground.GetLength(0); i++)
            {
                for (int j = 0; j < enemyPlayground.GetLength(1); j++)
                {
                    enemyPlayground[i, j] = '-';
                }
            }

            for (int i = 0; i < room.enemy.EnemyShips.Count; i++)
            {
                for (int j = 0; j < room.enemy.EnemyShips[i].Coords.Count; j++)
                {
                    enemyPlayground[room.enemy.EnemyShips[i].Coords[j].Y, room.enemy.EnemyShips[i].Coords[j].X] = '*';
                }
            }

            showLogPlayground();
        }
        private void showLogPlayground()
        {
            string logOutput = "";

            for (int i = 0; i < enemyPlayground.GetLength(0); i++)
            {
                for (int j = 0; j < enemyPlayground.GetLength(1); j++)
                {
                    logOutput += enemyPlayground[i, j] + " ";
                }
                logOutput += "\n";
            }

            _logger.LogInformation("enemy playground generated\n"
                                  + logOutput);
        }
    }
}

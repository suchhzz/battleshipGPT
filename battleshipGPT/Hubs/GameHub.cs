using battleshipGPT.Models.GameModels;
using battleshipGPT.Serives;
using battleshipGPT.Services;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;

namespace battleshipGPT.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;
        private readonly RoomService _roomService;
        private readonly ILogger<GameHub> _logger;
        public GameHub(GameService gameService, RoomService roomService, ILogger<GameHub> logger)
        {
            _gameService = gameService;
            _roomService = roomService;
            _logger = logger;
        }

        public async Task ConnectUser(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
        public async Task PlayerMove(string roomId, int selectedCol, int selectedRow)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var hitPointModel = _gameService.GetHitCoord(currentRoom, selectedCol, selectedRow, true);

            await Clients.Groups(roomId).SendAsync("SetClientPoint", hitPointModel.isHit, hitPointModel.HitCoords, hitPointModel.BorderCoords);

            await CheckWinner(roomId);
        }

        public async Task EnemyMove(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var enemyRandomPoint = _gameService.EnemySetPoint(currentRoom);

            var enemyHitPoint = _gameService.GetHitCoord(currentRoom, enemyRandomPoint.X, enemyRandomPoint.Y, false);

            await Clients.Group(roomId).SendAsync("SetEnemyPoint", enemyHitPoint.isHit, enemyHitPoint.HitCoords, enemyHitPoint.BorderCoords);

            await CheckWinner(roomId);
        }

        private async Task CheckWinner(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom.Enemy.EnemyShipsRemaining == 0)
            {
                await Clients.Group(roomId).SendAsync("GameOver", true);
            }
            else if (currentRoom.Player.PlayerShipsRemaining == 0)
            {
                await Clients.Group(roomId).SendAsync("GameOver", false);
            }
        }
    }
}

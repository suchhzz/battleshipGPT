using battleshipGPT.Models.GameModels;
using battleshipGPT.Serives;
using battleshipGPT.Services;
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
        public async Task PlayerMove(string roomId, int selectedCol, int selectedRow)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var hitPointModel = _gameService.GetHitCoord(currentRoom, selectedCol, selectedRow, true);

            _logger.LogInformation($"hitPointModel: is hit: {hitPointModel.isHit} hit coords: {hitPointModel.HitCoords.X} borderCoords: {hitPointModel.BorderCoords.Count}");

            await Clients.Caller.SendAsync("SetClientPoint", hitPointModel.isHit, hitPointModel.HitCoords, hitPointModel.BorderCoords);

            await CheckWinner(roomId);
        }

        public async Task EnemyMove(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var enemyRandomPoint = _gameService.EnemySetPoint(currentRoom);

            _logger.LogInformation($"enemy chose: x: {enemyRandomPoint.X} y: {enemyRandomPoint.Y}");

            var enemyHitPoint = _gameService.GetHitCoord(currentRoom, enemyRandomPoint.X, enemyRandomPoint.Y, false);

            _logger.LogInformation($"hitPointModel: is hit: {enemyHitPoint.isHit} hit coords: x: {enemyHitPoint.HitCoords.X} y: {enemyHitPoint.HitCoords.Y} borderCoords: {enemyHitPoint.BorderCoords.Count}");

            await Clients.Caller.SendAsync("SetEnemyPoint", enemyHitPoint.isHit, enemyHitPoint.HitCoords, enemyHitPoint.BorderCoords);

            await CheckWinner(roomId);
        }

        private async Task CheckWinner(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            if (currentRoom.enemy.EnemyShipsRemaining == 0)
            {
                await Clients.Caller.SendAsync("GameOver", true);
            }
            else if (currentRoom.Player.PlayerShipsRemaining == 0)
            {
                await Clients.Caller.SendAsync("GameOver", true);
            }
        }
    }
}

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
        public GameHub(GameService gameService, RoomService roomService)
        {
            _gameService = gameService;
            _roomService = roomService;
        }
        public async Task PlayerMove(string roomId, int selectedCol, int selectedRow)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var hitPointModel = _gameService.GetHitCoord(currentRoom, selectedCol, selectedRow);

            await Clients.Group(roomId).SendAsync("SetClientPoint", hitPointModel.isHit, hitPointModel.HitCoords, hitPointModel.BorderCoords);
        }

        public async Task EnemyMove(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            _gameService.EnemySetPoint(currentRoom);
        }

        public async Task TestHitPoint(string roomId)
        {
            var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            List<Coordinates> hitCoords = new List<Coordinates>();
            hitCoords.Add(new Coordinates { X = 4, Y = 4 });
            hitCoords.Add(new Coordinates { X = 7, Y = 5 });

            List<Coordinates> missCoords = new List<Coordinates>();
            missCoords.Add(new Coordinates { X = 6, Y = 6 });


            await Clients.Caller.SendAsync("SetClientPoint", hitCoords, missCoords);
        }
    }
}

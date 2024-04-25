using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Models.PlayersModel;
using battleshipGPT.Services;

namespace battleshipGPT.Serives
{
    public class RoomService
    {
        public List<Room> rooms = new List<Room>();

        public void CreateRoom(Player player, Guid roomId)
        {
            rooms.Add(new Room
            {
                Player = player,
                RoomId = roomId,
                enemy = new Enemy
                {
                    EnemyShips = new List<ShipModel>(),
                    UsedCoordinates = new List<Coordinates>(),
                    AvailableCoordinates = new List<Coordinates>(),
                    EnemyHitCoordinates = new List<Coordinates>(),
                    EnemyShipsRemaining = 0
                }
            });
        }

        public Room GetRoomById(Guid roomId)
        {
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }
    }
}

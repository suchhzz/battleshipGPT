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
                Enemy = new Enemy
                {
                    EnemyShips = new List<ShipModel>(),
                    UsedCoordinates = new List<Coordinates>(),
                    AvailableCoordinates = setAvailableCoordinates(),
                    EnemyHitCoordinates = new List<Coordinates>(),
                    EnemyShipsRemaining = 10
                }
            }) ;
        }

        private List<Coordinates> setAvailableCoordinates()
        {
            List<Coordinates> available = new List<Coordinates>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    available.Add(new Coordinates { X = j, Y = i });
                }
            }

            return available;
        }

        public Room GetRoomById(Guid roomId)
        {
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }
    }
}

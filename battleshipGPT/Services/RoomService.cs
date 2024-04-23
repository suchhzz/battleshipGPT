using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Services;

namespace battleshipGPT.Serives
{
    public class RoomService
    {
        public List<Room> rooms = new List<Room>();

        public void CreateRoom(User user, Guid roomId)
        {
            rooms.Add(new Room
            {
                User = user,
                RoomId = roomId,
                userShips = new List<ShipModel>(),
                enemyShips = new List<ShipModel>(),
                userUsedCoordinates = new List<Coordinates>(),
                enemyUsedCoordinates = new List<Coordinates>()
            }) ;
        }

        public Room GetRoomById(Guid roomId)
        {
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }
    }
}

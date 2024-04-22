using battleshipGPT.Models;
using battleshipGPT.Services;

namespace battleshipGPT.Serives
{
    public class RoomService
    {
        public List<Room> rooms = new List<Room>();

        public void CreateRoom(Guid userId, Guid roomId)
        {
            rooms.Add(new Room
            {
                UserId = userId,
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

using battleshipGPT.Models.GameModels;

namespace battleshipGPT.Models.MainModels
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public User User { get; set; }
        public List<ShipModel> userShips { get; set; }
        public List<ShipModel> enemyShips { get; set; }
        public int enemyShipsRemaining { get; set; }
        public int userShipsRemaining { get; set; }
        public List<Coordinates> userUsedCoordinates { get; set; }
        public List<Coordinates> enemyUsedCoordinates { get; set; }
        public List<Coordinates> enemyHitCoordinates { get; set; }
    }
}

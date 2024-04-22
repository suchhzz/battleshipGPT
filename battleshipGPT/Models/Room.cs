namespace battleshipGPT.Models
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public List<ShipModel> userShips { get; set; }
        public List<ShipModel> enemyShips { get; set; }
        public List<Coordinates> userUsedCoordinates { get; set; }
        public List<Coordinates> enemyUsedCoordinates { get; set; }
    }
}

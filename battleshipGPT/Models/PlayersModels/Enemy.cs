using battleshipGPT.Models.GameModels;

namespace battleshipGPT.Models.PlayersModel
{
    public class Enemy
    {
        public List<ShipModel> EnemyShips { get; set; }
        public int EnemyShipsRemaining { get; set; }
        public List<Coordinates> UsedCoordinates { get; set; }
        public List<Coordinates> AvailableCoordinates { get; set; }
        public List<Coordinates> EnemyHitCoordinates { get; set; }
    }
}

using battleshipGPT.Models.GameModels;

namespace battleshipGPT.Models.PlayersModel
{
    public class Player
    {
        public Guid Id { get; set; }
        public List<ShipModel> PlayerShips { get; set; }
        public int PlayerShipsRemaining { get; set; }
    }
}

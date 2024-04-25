using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.PlayersModel;

namespace battleshipGPT.Models.MainModels
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public Player Player { get; set; }
        public Enemy enemy { get; set; }
    }
}

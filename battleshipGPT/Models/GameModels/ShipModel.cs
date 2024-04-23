namespace battleshipGPT.Models.GameModels
{
    public class ShipModel
    {
        public List<Coordinates> Coords { get; set; }
        public int Deck { get; set; }
        public bool Horizontal { get; set; }
        public bool Destroyed { get; set; } = false;
        public int DeckRemaining { get; set; }
    }
}

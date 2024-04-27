namespace battleshipGPT.Models.GameModels
{
    public class HitPointModel
    {
        public Coordinates HitCoords { get; set; }
        public List<Coordinates> BorderCoords { get; set; }
        public bool isHit { get; set; }
    }
}

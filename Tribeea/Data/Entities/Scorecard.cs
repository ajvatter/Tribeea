namespace Tribeea.Data.Entities
{
    public class Scorecard
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int EventId { get; set; }
        public float RoundOneScore { get; set; }
        public float RoundTwoScore { get; set; }
        public float RoundThreeScore { get; set; }
        public float RoundFourScore { get; set; }

        public virtual Team? Team { get; set; }
        public virtual Event? Event { get; set; }
    }
}

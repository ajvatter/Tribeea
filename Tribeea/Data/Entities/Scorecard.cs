namespace Tribeea.Data.Entities
{
    public class Scorecard
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int EventId { get; set; }
        public int RoundOneScore { get; set; }
        public int RoundTwoScore { get; set; }
        public int RoundThreeScore { get; set; }
        public int RoundFourScore { get; set; }

        public virtual Team? Team { get; set; }
        public virtual Event? Event { get; set; }
    }
}

namespace Tribeea.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public string? MusicUrl { get; set; }
        public ICollection<Scorecard>? Scorecards { get; set; }
    }
}

namespace Tribeea.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Scorecard>? Scorecards { get; set; }
    }
}

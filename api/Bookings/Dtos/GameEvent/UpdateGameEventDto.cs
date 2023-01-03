namespace Bookings.Dtos
{
    public class UpdateGameEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Event Title/Name";
        public string ShortDescription { get; set; } = "Short description of event";
        public string EventType { get; set; } = "Competitive";
        public string Reward { get; set; } = "Nintendo Switch Game of Choice";
        public string Game { get; set; } = "Mario Kart 8 Deluxe";
        public string Platform { get; set; } = "Nintendo Switch";
        public DateTime StartTime { get; set; } = DateTime.Now.AddDays(7);
        public int TeamSize { get; set; } = 1;
        public string BracketType { get; set; } = "Single-Elimination Round Robin";
        public string RuleSet { get; set; } = "Standard Rules";
    }
}
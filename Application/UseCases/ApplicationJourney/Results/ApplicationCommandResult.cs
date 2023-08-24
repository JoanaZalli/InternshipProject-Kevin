namespace Application.UseCases.ApplicationJourney.Results {
    public class ApplicationCommandResult {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}

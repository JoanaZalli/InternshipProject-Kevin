namespace Application.UseCases.ApplicationJourney.Results {
    public class ApplicationQueryResult {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int RequestedAmount { get; set; }
        public int RequestedTenor { get; set; }
        public string FinancePurposeDefinition { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}

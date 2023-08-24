namespace InternshipProject.Objects.Requests.ApplicationJourneyRequests {
    public class CreateApplicationRequest {
        public int RequestedAmount { get; set; }
        public int RequestedTenor { get; set; }
        public string FinancePurposeDefinition { get; set; } = null!;
    }
}

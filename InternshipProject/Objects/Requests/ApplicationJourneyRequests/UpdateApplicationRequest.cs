namespace InternshipProject.Objects.Requests.ApplicationJourneyRequests {
    public class UpdateApplicationRequest {
        public int RequestedAmount { get; set; }
        public int RequestedTenor { get; set; }
        public string FinancePurposeDefinition { get; set; } = null!;
        public string ProductId { get; set; } = null!;
    }
}

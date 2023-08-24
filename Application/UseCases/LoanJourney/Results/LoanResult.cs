namespace Application.UseCases.LoanJourney.Results {
    public class LoanResult {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public decimal ReferenceRate { get; set; }
        public decimal InterestRate { get; set; }
        public int Tenor { get; set; }
        public string Status { get; set; } = null!;
    }
}

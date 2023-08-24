namespace Application.UseCases.BorrowerJourney.Results {
    public class BorrowerQueryResult {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string FiscalCode { get; set; } = null!;
    }
}

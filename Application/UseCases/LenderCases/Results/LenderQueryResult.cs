namespace Application.UseCases.LenderCases.Results {
    public class LenderQueryResult {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int RequestedAmount { get; set; }
        public int MinTenor { get; set; }
        public int MaxTenor { get; set; }
        public string BorrowerCompanyType { get; set; } = null!;
    }
}

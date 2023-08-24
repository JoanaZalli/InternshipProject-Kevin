namespace Application.UseCases.CompanyTypeCases.Results {
    public class CompanyTypeResult {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
    }
}

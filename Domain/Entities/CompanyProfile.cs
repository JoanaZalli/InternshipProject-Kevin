using Domain.Common;

namespace Domain.Entities {
    public class CompanyProfile : BaseEntity {
        public Guid Id { get; set; }
        public string? Country { get; set; }
        public string? Currency { get; set; }
        public string? Exchange { get; set; }
        public string? IPO { get; set; }
        public decimal? MarketCapitalization { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public decimal? ShareOutstanding { get; set; }
        public string? Ticker { get; set; }
        public string? WebUrl { get; set; }
        public string? Logo { get; set; }
        public string? FinnhubIndustry { get; set; }
        public Guid BorrowerId { get; set; }
        public virtual Borrower Borrower { get; set; } = null!;
    }
}

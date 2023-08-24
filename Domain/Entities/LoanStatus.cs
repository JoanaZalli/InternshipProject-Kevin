using Domain.Common;

namespace Domain.Entities {
    public class LoanStatus : BaseEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Loan> Loans { get; set; } = null!;
    }
}

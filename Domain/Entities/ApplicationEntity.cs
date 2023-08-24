using Domain.Common;

namespace Domain.Entities {
    public class ApplicationEntity : BaseEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int RequestedAmount { get; set; }
        public int RequestedTenor { get; set; }
        public string FinancePurposeDefinition { get; set; } = null!;
        public Guid BorrowerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ApplicationStatusId { get; set; }
        public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;
        public virtual Borrower Borrower { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Loan? Loan { get; set; }
    }
}

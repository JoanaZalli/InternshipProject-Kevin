using Domain.Common;

namespace Domain.Entities {
    public class CompanyType : BaseEntity {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Borrower> Borrowers { get; set; } = null!;
    }
}
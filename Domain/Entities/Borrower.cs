using Domain.Common;

namespace Domain.Entities {
    public class Borrower : BaseEntity {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string FiscalCode { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid CompanyTypeId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual CompanyType CompanyType { get; set; } = null!;
        public virtual CompanyProfile CompanyProfile { get; set; } = null!;
        public virtual ICollection<ApplicationEntity>? Applications { get; set; }
    }
}

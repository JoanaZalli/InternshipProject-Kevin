using Domain.Common;

namespace Domain.Entities {
    public class Product : BaseEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal ReferenceRate { get; set; }
        public decimal FinanceMaxAmount { get; set; }
        public decimal FinanceMinAmount { get; set; }
        public virtual ICollection<ApplicationEntity>? Applications { get; set; }
    }
}

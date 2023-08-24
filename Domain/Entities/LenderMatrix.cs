using Domain.Common;

namespace Domain.Entities {
    public class LenderMatrix : BaseEntity {
        public Guid Id { get; set; }
        public Guid LenderId { get; set; }
        public Guid ProductId { get; set; }
        public int Tenor { get; set; }
        public decimal Spread { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Domain.Common;

namespace Domain.Entities {
    public class ApplicationStatus : BaseEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<ApplicationEntity> Applications { get; set; } = null!;
    }
}

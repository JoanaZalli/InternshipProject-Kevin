using Domain.Common;

namespace Domain.Entities {
    public class Role : BaseEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; } = null!;
        public virtual ICollection<Permission> Permissions { get; set; } = null!;
    }
}

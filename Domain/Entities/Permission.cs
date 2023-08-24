using Domain.Common;

namespace Domain.Entities {
    public class Permission : BaseEntity {
        public Guid Id { get; set; }    
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = null!;
    }
}

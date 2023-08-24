using Domain.Common;

namespace Domain.Entities {
    public class User_Role : BaseEntity {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}

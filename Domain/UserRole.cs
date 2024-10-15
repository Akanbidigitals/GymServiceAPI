using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class UserRole
    {
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey (nameof(User))]
        public Guid UserId {  get; set; }
        public User User { get; set; }
    }
}

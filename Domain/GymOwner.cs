using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class GymOwner 
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }


        [Column(TypeName = "decimal(8,2)")]
        public decimal MonthlyEarnings { get; set; } = 0;
        
        public List<GymMember>? GymMembers { get; set; }
        public List<Payment>? Payments { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class GymSuperAdmin 
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [Column(TypeName = "decimal(3,2)")]

        public decimal MonthlyPercentage { get; set; }
        public List<GymOwner> Owners { get; set; }
        List<Payment> Payments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class GymMember
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }  
        

        public DateTime SubscriptionStart { get; set; }

        public DateTime SubscriptionEnd { get; set; } 

        [ForeignKey(nameof(GymOwner))]
        public Guid GymOwnerId { get; set; }
        public GymOwner GymOwner { get; set; }

        public List<Payment> Payments { get; set; }

    }
}

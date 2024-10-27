using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class Payment
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        //Gym owner 
        [ForeignKey(nameof(GymOwner))]
        public Guid GymOwerId { get; set; }
        public GymOwner GymOwner { get; set; }

        //Gym Member
        [ForeignKey(nameof(GymMember))]
        public Guid? GymMemberId { get; set; }
        public GymMember? GymMember { get; set; }


        //SuperAdmin
        [ForeignKey(nameof(GymSuperAdmin))]
        public Guid? GymSuperAdminId {  get; set; }
        public GymSuperAdmin? GymSuperAdmin { get; set; }


        //Descriptiom(Optional)
        public string Description {  get; set; } = string.Empty;
    }
}

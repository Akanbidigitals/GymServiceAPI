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

        public string PaymentType { get; set; }

        public string SenderAccount { get; set; }

        public string RecieverAccount { get; set; }
       

        

        //Generate Payment Reference for Tracking
        [Required]
        public string PaymentReference { get; set; }

        //Descriptiom(Optional)
        public string Description {  get; set; } = string.Empty;
    }
}

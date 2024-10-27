using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class HealthyTip
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

       
        
    }
}

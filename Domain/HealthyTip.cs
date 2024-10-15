using System.ComponentModel.DataAnnotations.Schema;

namespace GymMembershipAPI.Domain
{
    public class HealthyTip
    {
        
        public string Name { get; set; }    
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

       
        
    }
}

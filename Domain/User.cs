using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GymMembershipAPI.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]

        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [Required, Length(5, 5)]
        public string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; } = 0;
        public string VerificationToken { get; set; }
        public bool Isverified { get; set; }
        public string TokenExpiration { get; set; } = "";
        public string VerifiedAt { get; set; } = "";

        public List<UserRole> Roles { get; set; } = [];

        

    }
}

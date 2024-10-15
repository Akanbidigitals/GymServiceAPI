namespace GymMembershipAPI.DTO.Register_Login
{
    public class RegisterSuperAdminDTO
    {
        public   string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public decimal MonthlyPercentage { get; set; } 

    }
}

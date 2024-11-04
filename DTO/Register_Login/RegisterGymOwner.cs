namespace GymMembershipAPI.DTO.Register_Login
{
    public class RegisterGymOwner
    {
        public Guid SuperAdminId {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

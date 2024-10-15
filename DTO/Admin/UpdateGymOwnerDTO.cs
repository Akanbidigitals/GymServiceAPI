namespace GymMembershipAPI.DTO.Admin
{
    public class UpdateGymOwnerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
    }
}

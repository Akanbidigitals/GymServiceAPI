namespace GymMembershipAPI.DTO.GymMember
{
    public class MakePaymentToGymOwnerDTO
    {
        public Guid GymMemberId { get; set; }
        public Guid GymOwnerId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = "";
    }
}

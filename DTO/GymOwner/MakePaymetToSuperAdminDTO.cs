namespace GymMembershipAPI.DTO.GymOwner
{
    public class MakePaymetToSuperAdminDTO
    {
        public Guid GymOwnerId {  get; set; }
        public Guid GymSuperAdminId {  get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = "";

    }
}

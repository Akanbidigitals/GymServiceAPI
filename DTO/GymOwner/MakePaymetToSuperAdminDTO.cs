namespace GymMembershipAPI.DTO.GymOwner
{
    public class MakePaymetToSuperAdminDTO
    {
        public string SenderAccount {  get; set; }
        public string RecieverAccount {  get; set; }

        public decimal Amount { get; set; }

    }
}

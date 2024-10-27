using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.Service;

namespace GymMembershipAPI.DataAccess.Interface
{
    public interface IGymMemberRepository
    {
        Task<List<Payment>> ViewPaymentHistory(Guid id);
        Task<ResponseModel<string>> checkexpiry(Guid id);
        Task<List<HealthyTip>> ViewHealthyTips();
        Task<ResponseModel<string>> PayGymOwner(MakePaymentToGymOwnerDTO _pay);
        Task<ResponseModel<string>> FundYourAccount(FundAccountDTO _fund);
    }
}

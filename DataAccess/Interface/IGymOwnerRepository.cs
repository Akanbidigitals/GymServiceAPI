using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymOwner;
using GymMembershipAPI.Service;

namespace GymMembershipAPI.DataAccess.Interface
{
    public interface IGymOwnerRepository
    {
        Task<List<GymMember>> GetAllGymMembers();
        Task <ResponseModel<string>> DeleteMember(Guid memberId);
        Task <ResponseModel<string>> UpdateMember(GymMemberUpdateDTO member);
        Task <ResponseModel<string>> GetMember(Guid memberId);
        Task <ResponseModel<string>> DepositMoney(DepositMoneyDTO deposit);
        Task<ResponseModel<string>> CheckAccountBalance(Guid Id);
        Task<ResponseModel<string>> MemeberSubscriptionDetails(Guid id);
        Task<ResponseModel<string>> CheckMemberAcctBalance(Guid memeberId);
        Task<ResponseModel<string>> PostHealthTips(PostHealthyTipsDTO post);
        Task<ResponseModel<string>> PayToSuperAdmin(MakePaymetToSuperAdminDTO _pay);
        
        
    }
}

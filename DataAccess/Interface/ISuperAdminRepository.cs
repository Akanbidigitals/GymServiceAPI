using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.Admin;
using GymMembershipAPI.Service;

namespace GymMembershipAPI.DataAccess.Interface
{
    public interface ISuperAdminRepository
    {
        Task<List<GymOwner>> GetAllGymOwners();
        Task<ResponseModel<string>> DeleteGymOwner(Guid id);
        Task<ResponseModel<string>> UpdateGymOwner(UpdateGymOwnerDTO update);
        Task<ResponseModel<string>> GetNumbersOfgymOwner();
        Task <Payment> VeiwAllPaymentsbyGymOwner( string acct);
        Task<ResponseModel<string>> GetAccountNumber( Guid id);
        
    }
}

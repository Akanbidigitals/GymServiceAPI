using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.DTO.Register_Login;
using GymMembershipAPI.Service;

namespace GymMembershipAPI.DataAccess.Interfaces
{
    public interface IRegister_Login
    {
        Task<ResponseModel<string>> RegistSuperAdmin(RegisterSuperAdminDTO user);
        Task<ResponseModel<string>> RegistGymOwners(RegisterGymOwner user);
        Task<ResponseModel<string>> RegistGymMembers(RegisterGymMemberDTO user);
        Task<ResponseModel<string>> Login(LoginDTO login);
        
    }
}

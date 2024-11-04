using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.DataAccess.Interfaces;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.DTO.GymOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipAPI.Controllers
{
    public class GymOwnerController : Controller
    {
        private readonly IGymOwnerRepository _ctx;
        private readonly IRegister_Login _Login;
        public GymOwnerController(IGymOwnerRepository ctx, IRegister_Login login)
        {
            _Login = login;
            _ctx = ctx;
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpPost("RegisterGymMember")]
        public async Task<IActionResult> RegisterMember(RegisterGymMemberDTO _register)
        {
            try
            {
                var res = await _Login.RegistGymMembers(_register);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpPost("MakePaymentToSuperAdmin")]
        public async Task<IActionResult> PaySuperAdmin(MakePaymetToSuperAdminDTO _pay)
        {
            try
            {
                var res = await _ctx.PayToSuperAdmin(_pay);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [Authorize(Roles ="GymOwner")]
        [HttpPut("UpdateGymMemeber")]
        public async Task <IActionResult> UpdateMember(GymMemberUpdateDTO _update)
        {
            try
            {
                var res = await _ctx.UpdateMember(_update);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [Authorize(Roles ="GymOwner")]
        [HttpDelete("DeleteGymMember")]
        public async Task<IActionResult> DeleteGymMemeber (Guid id)
        {
            try
            {
                var res = await _ctx.DeleteMember(id);
                return Ok(res); 
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpGet("GetAllGymMemebers")]
        public async Task<IActionResult> GetAllGymMember()
        {
            try
            {
                var res = await _ctx.GetAllGymMembers();
                return Ok(res);
            }catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpGet("GetMember")]
        public async Task<IActionResult> GetMember(Guid id)
        {
            try
            {
                var res = await _ctx.GetMember(id);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpGet("CheckBalance")]
        public async Task<IActionResult> CheckBalance (Guid id)
        {
            try
            {
                var res = await _ctx.CheckAccountBalance(id);
                return Ok(res); 
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize(Roles ="GymOwner")]
        [HttpGet("CheckMemberBalance")]
        public async Task<IActionResult> CheckMemberBalance (Guid id)
        {
            try
            {
                var res = await _ctx.CheckMemberAcctBalance(id);
                return Ok(res); 
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpPost("DepositMoney")]
        public async Task<IActionResult> DepositMoney(FundAccountDTO fundAccountDTO)
        {
            try
            {
               var res = await _ctx.DepositMoney(fundAccountDTO);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymOwner")]
        [HttpGet("CheckMemeberSubscriptionDetails")]
        public async Task<IActionResult> CheckSubscription(Guid id)
        {
            try
            {
                var res = await _ctx.MemeberSubscriptionDetails(id);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message) ;
            }
        }
        
        [Authorize(Roles ="GymOwner")]
        [HttpPost("PostHealthyTips")]
        public async Task<IActionResult>PostHealthyTips(PostHealthyTipsDTO _post)
        {
            try
            {

                var res = await _ctx.PostHealthTips(_post);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

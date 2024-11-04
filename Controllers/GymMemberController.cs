using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.DTO.GymMember;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipAPI.Controllers
{
    public class GymMemberController : Controller
    {
        private readonly IGymMemberRepository _ctx;
        public GymMemberController(IGymMemberRepository ctx)
        {
            _ctx = ctx;
        }

        
        [Authorize(Roles ="GymMembers")]
        [HttpGet("ViewPaymentsHistory")]
        public async Task<IActionResult> GetPAymentHistory(Guid id)
        {
            try
            {
                var res = await _ctx.ViewPaymentHistory(id);
                return Ok(res);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymMembers")]
        [HttpGet("CheckSubscriptionExpiry")]
        public async Task<IActionResult> CheckSubHistory(Guid id)
        {
            try
            {
                var res = await _ctx.checkexpiry(id);
                return Ok(res);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize(Roles ="GymMembers")]
        [HttpGet("ViewHealtyTips")]
        public async Task<IActionResult> ViewHealthyTips()
        {
            try
            {
                var res = await _ctx.ViewHealthyTips();
                return Ok(res);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymMembers")]
        [HttpPost("PayToGymOwner")]
        public async Task<IActionResult> PayToGymOwner(MakePaymentToGymOwnerDTO _pay)
        {
            try
            {
                var res = await _ctx.PayGymOwner(_pay);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="GymMembers")]
        [HttpPost("FundWallet")]
        public async Task<IActionResult>FundWallet(FundAccountDTO _fund)
        {
            try
            {
                var res = await _ctx.FundYourAccount(_fund);
                return Ok(res);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

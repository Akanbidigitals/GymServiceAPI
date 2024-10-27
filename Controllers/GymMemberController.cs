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

        [Authorize(Roles = "GymMemebers")]
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

        [Authorize(Roles = "GymMemebers")]
        [HttpGet("CheckSubscriptionExpiry")]
        public async Task<IActionResult> checkSubHistory(Guid id)
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
        [Authorize(Roles = "GymMemebers")]
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

        [Authorize(Roles = "GymMemebers")]
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

        [Authorize(Roles = "GymMemebers")]
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

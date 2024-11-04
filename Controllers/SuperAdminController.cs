using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.DataAccess.Interfaces;
using GymMembershipAPI.DTO.Admin;
using GymMembershipAPI.DTO.Register_Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipAPI.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminRepository _ctx;
        private readonly IRegister_Login _Login;
        public SuperAdminController(ISuperAdminRepository ctx,IRegister_Login login)
        {
            _ctx = ctx;
            _Login = login;
        }

        [HttpPost("RegisterSuperAdmin")]

        public async Task<IActionResult> RegisterSuperAdmin(RegisterSuperAdminDTO register)
        {
            try
            {
                var res = await _Login.RegistSuperAdmin(register);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("RegisterGymOwner")]

        public async Task<IActionResult> RegisterGymOwner(RegisterGymOwner register)
        {
            try
            {
                var res = await _Login.RegistGymOwners(register);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        public async Task<IActionResult> ValidateToken(string token)
        {
            try
            {
                var res = await _Login.VerifyUser(token);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("RegenerateToken")]
        public async Task<IActionResult> RegenerateToken(string mail)
        {
            try
            {
                var res = await _Login.RegenerateToken(mail);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("GetListOfGymOwner")]

        public async Task<IActionResult> GetListOfGymOwner()
        {
            try
            {
                var res = await _ctx.GetAllGymOwners();
                return Ok(res);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        
        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("GetPaymentHistoryOfGymOwner")]
        
        public async Task<IActionResult>GetHistory(Guid id)
        {
            try
            {
                var res = await _ctx.VeiwAllPaymentsbyGymOwner(id);
                return Ok(res); 
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        
        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("GetTheCountOfGymOwner")]
        public async Task<IActionResult> GetGymOwnerCount()
        {
            try
            {
                var res = await _ctx.GetNumbersOfgymOwner();
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="SuperAdmin")]
        [HttpPut("UpdateGymOwner")]
        public async Task<IActionResult> UpdateGymOwner(UpdateGymOwnerDTO _dto)
        {
            try
            {
                var res = await _ctx.UpdateGymOwner(_dto);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles ="SuperAdmin")]
        [HttpDelete("DeleteGymOwner")]
        public async Task <IActionResult>DeleteGymOwner(Guid Id)
        {
            try
            {
                var res = await _ctx.DeleteGymOwner(Id);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteSuperAdmin")]
        public async Task<IActionResult>DeleteSuperAdmin(Guid id)
        {
            try
            {
                var res = await _ctx.DeleteSuperAdmin(id);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(LoginDTO _login)
        {
            try
            {
                var res = await _Login.Login(_login);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

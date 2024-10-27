using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.DTO.GymOwner;
using GymMembershipAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace GymMembershipAPI.DataAccess.Repository
{
    public class GymOwnerRepository(ApplicationContext ctx) : IGymOwnerRepository
    {
        private readonly ApplicationContext _ctx = ctx;

        public async Task<ResponseModel<string>> CheckAccountBalance(Guid id)
        {
            var response = new ResponseModel<string>();

            try
            {
                var verifyacctId = await _ctx.GymOwner.FirstOrDefaultAsync(x => x.Id == id);
                if (verifyacctId == null)
                {
                    response = response.FailedResult("User does not exist");
                }
                else
                {
                    var checkuser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == verifyacctId.Email);

                    response = response.SuccessResult($"Dear {checkuser.Name} your account balance is #{checkuser.AccountBalance}");
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<string>> CheckMemberAcctBalance(Guid memeberId)
        {
            var response = new ResponseModel<String>();
            try
            {
                var verifyMember = await _ctx.Members.FirstOrDefaultAsync(x=>x.Id == memeberId);
                if(verifyMember == null)
                {
                    response = response.FailedResult("User does not exist");

                }
                else
                {
                    var checkuser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == verifyMember.Email);
                    response = response.SuccessResult($"Your GymMember {checkuser.Name} account balance is #{checkuser.AccountBalance}");
                }
            }
            catch
            {
                throw;
            }
            return response;    
        }

        public async Task<ResponseModel<string>> DeleteMember(Guid memberId)
        {

            var response = new ResponseModel<String>();
            try
            {
                var verifyMember = await _ctx.Members.FirstOrDefaultAsync(x=>x.Id ==  memberId);
                if (verifyMember == null)
                {
                    response = response.FailedResult("User does not exist");

                }
                else
                {
                    var checkuser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == verifyMember.Email);
                   _ctx.Members.Remove(verifyMember);
                    _ctx.Users.Remove(checkuser!);
                    await _ctx.SaveChangesAsync();

                    response = response.SuccessResult("Memeber deleted Successfully");
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel<string>> DepositMoney(FundAccountDTO deposit)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x => x.Id == deposit.Id);
                if(checkOwner == null)
                {
                    response = response.FailedResult("Gym Owner does not exist");
                }
                var gymOwnerUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkOwner.Email);
                gymOwnerUser.AccountBalance += deposit.Amount;
                _ctx.Users.Update(gymOwnerUser);
                await _ctx.SaveChangesAsync();
                response = response.SuccessResult($"Dear {checkOwner.Name} you have successfully deposited {deposit.Amount} to your account.\nBalance : #{gymOwnerUser.AccountBalance}");

            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<List<GymMember>> GetAllGymMembers() => await _ctx.Members.ToListAsync();

        public async Task<ResponseModel<string>> GetMember(Guid memberId)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkmember = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == memberId);
                if(checkmember == null)
                {
                    response = response.FailedResult("member Id does not exist");
                }
                response = response.SuccessResult($"{checkmember}");
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel<string>> MemeberSubscriptionDetails(Guid id)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkmemberdetails = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == id);
                if(checkmemberdetails == null)
                {
                    response = response.FailedResult("Id does not exist, pls check");
                }
                var subscriptiondate = checkmemberdetails.SubscriptionStart.ToString("D");
                var subscriptionend = checkmemberdetails.SubscriptionEnd.ToString("D");

                response = response.SuccessResult($"Hello {checkmemberdetails.Name}.\nSubscription-Date : {subscriptiondate}.\nSubscription-Ends : {subscriptionend}");
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel<string>> PayToSuperAdmin(MakePaymetToSuperAdminDTO _pay)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkGymOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x => x.Id == _pay.GymOwnerId);
                var checkSuperAdmin = await _ctx.GymSuperAdmins.FirstOrDefaultAsync(x => x.Id == _pay.GymSuperAdminId);
                if( checkSuperAdmin == null || checkGymOwner == null)
                {
                    response = response.FailedResult("Gym Owner Id or GymSuper Id does not exist");
                }
                else
                {
                    var savepayment = new Payment()
                    {
                        GymOwerId = checkGymOwner.Id,
                        GymOwner = checkGymOwner,
                        GymSuperAdminId = checkSuperAdmin.Id,
                        GymSuperAdmin = checkSuperAdmin,
                        Amount = _pay.Amount,
                        Description = _pay.Description,


                    };
                    await _ctx.Payment.AddAsync(savepayment);
                   var res =  await _ctx.SaveChangesAsync();
                    if(res > 0)
                    {
                        var SuperUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkSuperAdmin.Email);
                        var OwnerUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkGymOwner.Email);

                        OwnerUser.AccountBalance -= _pay.Amount;
                        SuperUser.AccountBalance += _pay.Amount;

                        _ctx.Users.Update(OwnerUser);
                        _ctx.Users.Update(SuperUser);
                        await _ctx.SaveChangesAsync();

                        response = response.SuccessResult($"Dear {checkGymOwner.Name} you have successfully transferred {_pay.Amount} to your SuperAdmin");
                    }

                }
                
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel<string>> PostHealthTips(PostHealthyTipsDTO post)
        {
            var response = new ResponseModel<string>();
            try
            {
                var healthtipspost = new HealthyTip()
                {
                    Name = post.Name,
                    Title = post.Title,
                    Content = post.Content
                };
               await _ctx.HealthyTip.AddAsync(healthtipspost);
               await _ctx.SaveChangesAsync();
                response = response.SuccessResult("Health tips posted successfull");
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel<string>> UpdateMember(GymMemberUpdateDTO member)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkmember = await _ctx.Members.FirstOrDefaultAsync(x=>x.Id == member.Id);
                if(checkmember == null)
                {
                    response.FailedResult("User does not exist");
                }

                var updatemember = new GymMember()
                {
                    Name = member.Name,
                };
                _ctx.Members.Update(updatemember);
                await _ctx.SaveChangesAsync();
                response = response.SuccessResult("Memeber sucessfully updated");

            }
            catch
            {
                throw;
            }
            return response;    
        }
    }
}

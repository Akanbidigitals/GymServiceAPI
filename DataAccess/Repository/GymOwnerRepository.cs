using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.Domain;
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

        public async Task<ResponseModel<string>> DepositMoney(DepositMoneyDTO deposit)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkuser = await _ctx.Users.FirstOrDefaultAsync(x=>x.AccountNumber == deposit.AccountNumber);
                if(checkuser == null)
                {
                    response = response.FailedResult("Invalid Account,pls check");
                }
                else
                {
                    checkuser.AccountBalance += deposit.Amount;

                    _ctx.Users.Update(checkuser);
                    await _ctx.SaveChangesAsync();
                    response = response.SuccessResult($"Deposit Successfull.\nAmount transfered #{deposit.Amount}.\nPresent Balance #{checkuser.AccountBalance}");


                }
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
                var sender = await _ctx.Users.FirstOrDefaultAsync(x => x.AccountNumber == _pay.SenderAccount);
                var reciever = await _ctx.Users.FirstOrDefaultAsync(x => x.AccountNumber == _pay.RecieverAccount);
                if(sender  == null || reciever == null)
                {
                    response = response.FailedResult("Account number is incorrect, pls check");
                }
                else if(sender.AccountBalance < _pay.Amount)
                {
                    response = response.FailedResult("Insufficient fund, pls fund your wallet");
                }
                else
                {
                    sender.AccountBalance -=  _pay.Amount;
                    reciever.AccountBalance += _pay.Amount;

                    _ctx.Users.Update(sender);
                    _ctx.Users.Update(reciever);
                    await _ctx.SaveChangesAsync();

                    response = response.SuccessResult($"Hello {sender.Name}, You have successfully transfered {_pay.Amount} to {reciever.Name}");
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

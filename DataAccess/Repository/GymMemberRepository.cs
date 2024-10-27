using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace GymMembershipAPI.DataAccess.Repository
{
    public class GymMemberRepository(ApplicationContext ctx) : IGymMemberRepository
    {
        private readonly ApplicationContext _ctx = ctx;

        public async Task<ResponseModel<string>> checkexpiry(Guid id)
        {   
            var response = new ResponseModel<string>();
            try
            {
                var check = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == id);
                if(check == null) 
                {
                    response = response.FailedResult("Gym member does not exist");
                }
                else
                {
                    response = response.SuccessResult($"Your subscription expires {check.SubscriptionEnd}");
                }

            }catch(Exception ex)
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<string>> FundYourAccount(FundAccountDTO _fund)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkMember = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == _fund.Id);
                if(checkMember == null)
                {
                    response = response.FailedResult("Id does not exist");
                }
                var memberUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkMember.Email);

                memberUser.AccountBalance += _fund.Amount;

                _ctx.Users.Update(memberUser);
                await _ctx.SaveChangesAsync();

                response = response.SuccessResult($"Dear {checkMember.Name} you have succesffuly funded your account with {_fund.Amount}\nYour Balance : #{memberUser.AccountBalance}.");

            }catch(Exception ex)
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<string>> PayGymOwner(MakePaymentToGymOwnerDTO _pay)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkGymOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x=>x.Id == _pay.GymOwnerId);
                var checkMember = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == _pay.GymMemberId);
                if( checkMember == null || checkGymOwner == null)
                {
                    response = response.FailedResult("Gym Member Id or gym owner Id is invalid");
                }
                else
                {
                    var addPayment = new Payment()
                    {
                        GymMemberId = checkMember.Id,
                        GymMember = checkMember,
                        GymOwerId = checkGymOwner.Id,
                        GymOwner = checkGymOwner,
                        Description = _pay.Description,

                    };

                    await _ctx.Payment.AddAsync(addPayment);
                    var res =  await _ctx.SaveChangesAsync();
                    if(res > 0)
                    {
                        var MemeberUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkMember.Email);
                        var OwnerUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkGymOwner.Email);

                        OwnerUser.AccountBalance -= _pay.Amount;
                        MemeberUser.AccountBalance += _pay.Amount;

                        _ctx.Users.Update(OwnerUser);
                        _ctx.Users.Update(MemeberUser);

                        await _ctx.SaveChangesAsync();
                        response = response.SuccessResult($"Dear {checkMember.Name} you have successfully sent {_pay.Amount} to your GymOwner");
                    }
                }

            }catch(Exception ex)
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }

        public async Task<List<HealthyTip>> ViewHealthyTips()
        {
            try
            {

            var healthtips = await _ctx.HealthyTip.ToListAsync();
            if(healthtips.Count() == 0)
            {
                throw new Exception("There is no healthy tips post");
            }
            else
            {
                return healthtips;
            }
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        public async Task<List<Payment>> ViewPaymentHistory(Guid id)
        {
            try
            {
                var checkid = await _ctx.Members.FirstOrDefaultAsync(x => x.Id == id);
                if(checkid == null)
                {
                    throw new Exception("Id does not exist");
                }
                else
                {
                    var checkUserPayments = await  _ctx.Payment.Where( x=> x.GymMember == checkid ).ToListAsync();

                    return checkUserPayments;
                    
                    
                }

            }catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }
    }
}

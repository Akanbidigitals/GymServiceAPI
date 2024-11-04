using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.Admin;
using GymMembershipAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace GymMembershipAPI.DataAccess.Repository
{
    public class SuperAdminRepository(ApplicationContext ctx) : ISuperAdminRepository
    {
        private readonly ApplicationContext _ctx = ctx;

        public async Task<ResponseModel<string>> DeleteGymOwner(Guid id)
        {  
            var response = new ResponseModel<string>(); 
            try
            {
                var checkGymOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x => x.Id == id);
                var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkGymOwner.Email);
                if (checkGymOwner == null)
                {
                    response = response.FailedResult("User does not exist");
                }
                 _ctx.GymOwner.Remove(checkGymOwner!);
                 _ctx.Users.Remove(user!);
                 await _ctx.SaveChangesAsync();
                response = response.FailedResult("Deleted Successfully");

            }
            
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return response;
        }

        public async Task<ResponseModel<string>> DeleteSuperAdmin(Guid id)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkId = await _ctx.GymSuperAdmins.FirstOrDefaultAsync(x => x.Id == id);
                if (checkId == null)
                {
                    response = response.FailedResult("Id does not exist");
                }
                var checkUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == checkId.Email);

                _ctx.GymSuperAdmins.Remove(checkId!);
                _ctx.Users.Remove(checkUser!);
                await _ctx.SaveChangesAsync();

                response = response.SuccessResult("SuperAdmin deleted Successfully");


            }catch(Exception ex)
            {
                response = response.FailedResult($"{ex.Message}");
            }
            return response;
        }

        public async Task<ResponseModel<string>> GetAccountNumber( Guid id)
        {
            var response = new ResponseModel<string>();
            try
            {
                var getUserAcct = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
                if(getUserAcct == null)
                {
                    response = response.FailedResult("User does not exist");
                }
                response = response.SuccessResult($"User Account Number : {getUserAcct.AccountNumber}.");
            }catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            return response;
        }


        public async Task<List<GymOwner>> GetAllGymOwners() => await _ctx.GymOwner.ToListAsync();

        public async Task<ResponseModel<string>> GetNumbersOfgymOwner()
        {
            var response = new ResponseModel<string>();
            var getnumbers = await _ctx.GymOwner.ToListAsync();
            int totalNumber = getnumbers.Count();
            response = response.SuccessResult($"{totalNumber} numbers");
            return response;
        }

        public async Task<ResponseModel<string>> UpdateGymOwner(UpdateGymOwnerDTO update)
        {
            var response = new ResponseModel<string>();
            try
            {
                var updateGymOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x => x.Id == update.Id);
                var updateUser = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == update.Email);
                if(updateGymOwner == null || updateUser == null )
                {
                    response = response.FailedResult("User does not exist");
                }
                //Update from gymowner domain
                updateGymOwner.Id = update.Id;
                updateGymOwner.Email = update.Email;
                updateGymOwner.Name = update.Name;
                //update fro user table
                updateUser.Name = update.Name;
                updateUser.Email = update.Email;

                _ctx.GymOwner.Update(updateGymOwner);
                _ctx.Users.Update(updateUser);
                await _ctx.SaveChangesAsync();
                response = response.SuccessResult("Profile updated successfully");


                 

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<List<Payment>> VeiwAllPaymentsbyGymOwner(Guid Id)
        {
            try
            {
                var checkId = await _ctx.GymOwner.FirstOrDefaultAsync(x=>x.Id == Id);
                if(checkId == null)
                {
                    throw new Exception("Gym Owner Id does not exist");
                }
                var memberUser = await _ctx.Payment.Where(x => x.GymOwner == checkId).ToListAsync();
                return memberUser;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

           
        }
    }
}

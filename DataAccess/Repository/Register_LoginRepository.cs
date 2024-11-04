using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interfaces;
using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.DTO.Mail;
using GymMembershipAPI.DTO.Register_Login;
using GymMembershipAPI.Mail;
using GymMembershipAPI.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymMembershipAPI.DataAccess.Repository
{
    public class Register_LoginRepository : IRegister_Login
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationContext _ctx;
        private readonly Setup _setup;
        public Register_LoginRepository(IEmailService emailService,IOptions<Setup> setup, ApplicationContext ctx)
        {
            _ctx = ctx;
            _emailService = emailService;
            _setup = setup.Value;
        }

        public async Task<ResponseModel<string>> Login(LoginDTO login)
        {
            var response = new ResponseModel<string>();
            try
            {
                var user = await _ctx.Users.Include(x => x.Roles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == login.Email);
                var verifypass = Utils.VerifyHashPasswod(login.Password, user.PasswordHash);
                if(user == null || !verifypass)
                {
                    response = response.FailedResult("Email or password is incorrect, pls try again");
                }
                else
                {
                    var userRoles = user.Roles.Select(x=> x.Role.Name).ToList();
                    //Claims
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name , user.Name),
                        new(ClaimTypes.Email, user.Email),
                    };
                    foreach(var role in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setup.SecretKey));
                    var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _setup.Issuer,
                        signingCredentials: credentials,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(120));

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    response = response.SuccessResult(jwtToken);
                }
                return response;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseModel<string>> RegenerateToken(string email)
        {
            var response = new ResponseModel<string>();
            try
            {
                var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == email);
                if(user == null)
                {
                    response = response.FailedResult("User does not exist");
                }else if(user.Isverified == true)
                {
                    response = response.FailedResult("You account is already verified");
                }
                else if (DateTime.Now > DateTime.Parse(user.TokenExpiration))
                {
                    user.TokenExpiration = "";
                    user.VerificationToken = "";
                    user.VerifiedAt = "Unverified";

                    _ctx.Users.Update(user);
                    await _ctx.SaveChangesAsync();
                    response = response.FailedResult("Token expired");
                }
                else
                {
                    user.Isverified = true;
                    user.VerifiedAt = DateTime.UtcNow.ToString("g");
                    user.VerificationToken = "";
                    user.TokenExpiration = "";
                    user.Roles = user.Roles;

                    _ctx.Users.Update(user);
                    await _ctx.SaveChangesAsync();
                    response = response.SuccessResult("Verification successfull");


                }

            }
            catch( Exception ex )
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<string>> RegistGymMembers(RegisterGymMemberDTO user)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkuser = await _ctx.Users.AnyAsync(x => x.Email == user.Email);
                if (checkuser)
                {
                    response = response.FailedResult("User already exist");
                }
                else
                {
                    var adduser = new User()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        PasswordHash = Utils.HashPassword(user.Password),
                        VerificationToken = Utils.CreateVerificationToken().Substring(0, 5),
                        VerifiedAt = "Unverified",
                        AccountBalance = 0,
                        AccountNumber = Utils.GenerateAcctNumber(),

                    };

                    await _ctx.Users.AddAsync(adduser);
                    await _ctx.SaveChangesAsync();

                    var role = await _ctx.Roles.FirstOrDefaultAsync(x => x.Id == 3);

                    var userrole = new UserRole()
                    {
                        RoleId = role.Id,
                        Role = role,
                        User = adduser,
                        UserId = adduser.Id

                    };

                    adduser.Roles.Add(userrole);
                    var res = await _ctx.SaveChangesAsync();
                    if (res > 0)
                    {
                        var checkGymOwner = await _ctx.GymOwner.FirstOrDefaultAsync(x=>x.Id == user.GymOwnerId);
                        if(checkGymOwner is null)
                        {
                            response = response.FailedResult("Gym Owner Id does not exist");
                        }
                        var addGymMemeber = new GymMember()
                        {
                            Email = adduser.Email,
                            Name = adduser.Name,
                            GymOwnerId = user.GymOwnerId,
                            GymOwner = checkGymOwner!

                    };
                       

                        await _ctx.Members.AddAsync(addGymMemeber);
                        var savedAdmin = await _ctx.SaveChangesAsync();
                        response = response.SuccessResult("Account created successfully");


                        //if (savedAdmin > 0)
                        //{

                        //    var sendmail = new MailRequest()
                        //    {
                        //        Reciever = addsuperadmin.Email,
                        //        Subject = "verify your email",
                        //        Content = adduser.VerificationToken
                        //    };
                        //    await _emailService.SendEmailAsync(sendmail);
                        //    response = response.SuccessResult("account created succesffully, pls kindly verify your email");


                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                return response;
        }

        public async Task<ResponseModel<string>> RegistGymOwners(RegisterGymOwner user)
        {
            var response = new ResponseModel<string>();
            try
            {
                var checkuser = await _ctx.Users.AnyAsync(x => x.Email == user.Email);
                if (checkuser)
                {
                    response = response.FailedResult("User already exist");
                }
                else
                {
                    var adduser = new User()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        PasswordHash = Utils.HashPassword(user.Password),
                        VerificationToken = Utils.CreateVerificationToken().Substring(0, 5),
                        VerifiedAt = "Unverified",
                        AccountBalance = 0,
                        AccountNumber = Utils.GenerateAcctNumber(),

                    };

                    await _ctx.Users.AddAsync(adduser);
                    await _ctx.SaveChangesAsync();

                    var role = await _ctx.Roles.FirstOrDefaultAsync(x => x.Id == 2);

                    var userrole = new UserRole()
                    {
                        RoleId = role.Id,
                        Role = role,
                        User = adduser,
                        UserId = adduser.Id

                    };

                    adduser.Roles.Add(userrole);
                    var res = await _ctx.SaveChangesAsync();
                    if (res > 0)
                    {
                        var checkSuperAdmin = await _ctx.GymSuperAdmins.FirstOrDefaultAsync(x => x.Id == user.SuperAdminId);
                        if(checkSuperAdmin is null)
                        {
                            response = response.FailedResult("SuperAdmin Id does not exist");
                        }
                        var addGymOwner = new GymOwner()
                        {
                            Email = adduser.Email,
                            Name = adduser.Name,
                            SuperAdminId = checkSuperAdmin.Id,
                            GymSuperAdmin = checkSuperAdmin
                            
                        };

                        await _ctx.GymOwner.AddAsync(addGymOwner)   ;
                        await _ctx.SaveChangesAsync();
                        response = response.SuccessResult("Account created successfully");


                        //if (savedAdmin > 0)
                        //{

                        //    var sendmail = new MailRequest()
                        //    {
                        //        Reciever = addsuperadmin.Email,
                        //        Subject = "verify your email",
                        //        Content = adduser.VerificationToken
                        //    };
                        //    await _emailService.SendEmailAsync(sendmail);
                        //    response = response.SuccessResult("account created succesffully, pls kindly verify your email");


                        //}
                    }
                }
            }
            catch(Exception ex)
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }
       

        public async Task<ResponseModel<string>> RegistSuperAdmin(RegisterSuperAdminDTO user)
        {

            var response = new ResponseModel<string>();
            try
            {
                var checkuser = await _ctx.Users.AnyAsync(x => x.Email == user.Email);
                if(checkuser)
                {
                    response = response.FailedResult("User already exist");
                }
                else
                {
                    var adduser = new User()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        PasswordHash = Utils.HashPassword(user.Password),
                        VerificationToken = Utils.CreateVerificationToken().Substring(0, 5),
                        VerifiedAt = "Unverified",
                        AccountBalance = 0,
                        AccountNumber = Utils.GenerateAcctNumber(),

                    };

                    await _ctx.Users.AddAsync(adduser);
                     await _ctx.SaveChangesAsync();

                    var role = await _ctx.Roles.FirstOrDefaultAsync(x=>x.Id == 1);

                    var userrole = new UserRole()
                    {
                        RoleId = role.Id,
                        Role = role,
                        User = adduser,
                        UserId = adduser.Id

                    };

                    adduser.Roles.Add(userrole);
                    var res = await _ctx.SaveChangesAsync();
                    if (res > 0)
                    {
                        var addsuperadmin = new GymSuperAdmin()
                        {
                            Email = adduser.Email,
                            Name = adduser.Name,
                            MonthlyPercentage = 0.20m
                        };

                        await _ctx.GymSuperAdmins.AddAsync(addsuperadmin);
                        var savedAdmin = await _ctx.SaveChangesAsync();

                        response = response.SuccessResult("Account created successfully");

                        //SMTP Server is down
                        //if(savedAdmin > 0)
                        //{
                            
                        //    var sendmail = new MailRequest()
                        //    {
                        //        Reciever = addsuperadmin.Email,
                        //        Subject = "verify your email",
                        //        Content = adduser.VerificationToken
                        //    };
                        //      await _emailService.SendEmailAsync(sendmail);
                        //    response = response.SuccessResult("account created succesffully, pls kindly verify your email");


                        //}
                    }
                }
            }catch( Exception ex )
            {
                response = response.FailedResult($"{ex.Message}");
            }
            return response;
        }
     
                       

                            
                            
                             
                           

                 

        public async Task<ResponseModel<string>> VerifyUser(string token)
        {
            var response = new ResponseModel<string>();
            try
            {
                var user = await _ctx.Users.FirstOrDefaultAsync(X => X.VerificationToken == token);
                if(user == null)
                {
                    response = response.FailedResult("Invalid token");
                }else if( DateTime.Now > DateTime.Parse(user.TokenExpiration))
                {
                    user.TokenExpiration = "";
                    user.VerificationToken = "";
                    user.VerifiedAt = "Unverified";

                    _ctx.Users.Update(user);
                    await _ctx.SaveChangesAsync();
                    response = response.FailedResult("Token expired");
                }
                else
                {
                    user.Isverified = true;
                    user.VerifiedAt = DateTime.UtcNow.ToString("g");
                    user.VerificationToken = "";
                    user.TokenExpiration = "";
                    user.Roles = user.Roles;

                    _ctx.Users.Update(user);
                    await _ctx.SaveChangesAsync();
                    response = response.SuccessResult("Verification successfull");
                   
                   
                }

            }catch(Exception ex)
            {
                response = response.FailedResult(ex.Message);
            }
            return response;
        }
    }
}

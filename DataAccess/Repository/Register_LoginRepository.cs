using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interfaces;
using GymMembershipAPI.Domain;
using GymMembershipAPI.DTO.GymMember;
using GymMembershipAPI.DTO.Mail;
using GymMembershipAPI.DTO.Register_Login;
using GymMembershipAPI.Mail;
using GymMembershipAPI.Service;
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
                }else if (!user.Isverified)
                {
                    response = response.FailedResult("Account not verified,Pls verify your accounr");
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
        
        public async Task<ResponseModel<string>> RegistGymMembers(RegisterGymMemberDTO user)
        {
            var response = new ResponseModel<string>();
            try
            {
                var newGymMember = await _ctx.Members.AnyAsync(x => x.Email == user.Email);
                var gymOwner = await _ctx.GymOwner.FirstAsync();
                
                if(gymOwner.Id == null)
                {
                    response = response.FailedResult("Gym OwnerId is invalid");
                }
                if (newGymMember)
                {
                    response = response.FailedResult("User already exist,pls login");
                }
                else
                {
                    var addnewGymMember = new GymMember()
                    {
                        GymownerId = gymOwner.Id,
                        Email = user.Email,
                        GymOwner = gymOwner,
                       

                    };
                    await _ctx.Members.AddAsync(addnewGymMember);
                    var result = await _ctx.SaveChangesAsync();
                    if(result > 0)
                    {
                        var adduser = new User()
                        {
                            Email = addnewGymMember.Email,
                            PasswordHash = Utils.HashPassword(user.Password),
                            VerificationToken = Utils.CreateVerificationToken().Substring(0,5),
                            VerifiedAt = "Unverified",
                            TokenExpiration = DateTime.Now.AddMinutes(20).ToString()

                        };

                        await _ctx.Users.AddAsync(adduser);
                        var userResult = await _ctx.SaveChangesAsync();

                        var assignmemberRole = await _ctx.Roles.FirstOrDefaultAsync(x => x.Id == 3);

                        var memberUserRole = new UserRole()
                        {
                            RoleId = assignmemberRole.Id,
                            Role = assignmemberRole,
                            UserId = adduser.Id,
                            User = adduser
                        };
                        adduser.Roles.Add(memberUserRole);
                       var res = await _ctx.SaveChangesAsync();
                        //Send mail
                        if(res > 0)
                        {
                            var bodyMsg = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Verification</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;\">\r\n    <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd;\">\r\n        <tr>\r\n            <td align=\"center\" style=\"padding: 20px 0;\">\r\n                <h2 style=\"color: #333; margin: 0;\">Email Verification</h2>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 10px 20px; color: #555; font-size: 16px; line-height: 1.5; text-align: center;\">\r\n                <p>Thank you {adduser.Name} for signing up! Please use the OTP code below to verify your email address. This code will expire in 5 minutes : {DateTime.Now.AddMinutes(5).ToString("g")}.</p>\r\n                <p style=\"font-size: 18px; font-weight: bold; color: #007bff; margin: 20px 0; text-align: center;\">\r\n                    <span style=\"background-color: #f0f0f0; padding: 10px 20px; border: 1px solid #ddd; border-radius: 5px; display: inline-block;\">{adduser.VerificationToken}</span>\r\n                </p>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 20px 0; text-align: center; color: #777; font-size: 12px;\">\r\n                <p>If you did not request this, please ignore this email.</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n";

                            var sendMail = new MailRequest()
                            {
                                Reciever = user.Email,
                                Subject = "Verify your email",
                                Content = adduser.VerificationToken
                            };
                            
                            await _emailService.SendEmailAsync(sendMail);
                            response = response.SuccessResult("Account created succesffully, pls kindly verify your email");

                        }
                       
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseModel<string>> RegistGymOwners(RegisterGymOwner user)
        {
            var response = new ResponseModel<string>();
            try
            {
                var gymowner = await _ctx.GymOwner.AnyAsync(x => x.Email == user.Email);
                if (gymowner)
                {
                    response = response.FailedResult("Email already exist, pls login");
                }
                var addGymMember = new GymOwner()
                {
                    Email = user.Email,
                    Name = user.Name,
                };

                await _ctx.GymOwner.AddAsync(addGymMember);
                await _ctx.SaveChangesAsync();

                var addUser = new User()
                {
                    Email = user.Email,
                    PasswordHash = Utils.HashPassword(user.Password),
                    VerificationToken = Utils.CreateVerificationToken().Substring(0, 5),
                    VerifiedAt = "Unverified",
                    AccountBalance = 0,
                    AccountNumber = Utils.GenerateAcctNumber(),

                };
                await _ctx.Users.AddAsync(addUser);
                await _ctx.SaveChangesAsync();

                var ownerRole = await _ctx.Roles.FirstOrDefaultAsync(x => x.Id == 3);

                var ownerUserRole = new UserRole()
                {
                    RoleId = ownerRole.Id,
                    Role = ownerRole,
                    UserId = addUser.Id,
                    User = addUser
                };

                addUser.Roles.Add(ownerUserRole);
                var res = await _ctx.SaveChangesAsync();
                if(res > 0)

                {
                    var bodyMsg = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Verification</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;\">\r\n    <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd;\">\r\n        <tr>\r\n            <td align=\"center\" style=\"padding: 20px 0;\">\r\n                <h2 style=\"color: #333; margin: 0;\">Email Verification</h2>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 10px 20px; color: #555; font-size: 16px; line-height: 1.5; text-align: center;\">\r\n                <p>Thank you {addUser.Name} for signing up! Please use the OTP code below to verify your email address. This code will expire in 5 minutes : {DateTime.Now.AddMinutes(5).ToString("g")}.</p>\r\n                <p style=\"font-size: 18px; font-weight: bold; color: #007bff; margin: 20px 0; text-align: center;\">\r\n                    <span style=\"background-color: #f0f0f0; padding: 10px 20px; border: 1px solid #ddd; border-radius: 5px; display: inline-block;\">{addUser.VerificationToken}</span>\r\n                </p>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 20px 0; text-align: center; color: #777; font-size: 12px;\">\r\n                <p>If you did not request this, please ignore this email.</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n";

                    var sendMail = new MailRequest
                    {
                        Reciever = user.Email,
                        Subject = "Verify your Email",
                        Content = bodyMsg,

                    };

                    await _emailService.SendEmailAsync(sendMail);
                    response = response.SuccessResult("Account successfull, pls check youe mail to verify");
                }
                return response;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseModel<string>> RegistSuperAdmin(RegisterSuperAdminDTO user)
        {
            var response = new ResponseModel<string>();

            try
            {
                var Newuser = await _ctx.GymSuperAdmins.AnyAsync(x=>x.Email == user.Email);
                if(Newuser)
                {
                    response = response.FailedResult("Email already exist");
                }
                else
                {
                    var newsuperadmin = new GymSuperAdmin()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        MonthlyPercentage = 0.20m
                        

                    };

                    await _ctx.GymSuperAdmins.AddAsync(newsuperadmin);
                    var res = await _ctx.SaveChangesAsync();
                    if(res > 0)
                    {
                        var newUser = new User()
                        {
                            Email = newsuperadmin.Email,
                            AccountNumber = Utils.GenerateAcctNumber(),
                            AccountBalance = 0,
                            PasswordHash = Utils.HashPassword(user.Password),
                            VerificationToken = Utils.CreateVerificationToken().Substring(0,5),
                            VerifiedAt = "Unverified",
                            TokenExpiration = DateTime.Now.AddMinutes(10).ToString()
                        };
                        await _ctx.Users.AddAsync(newUser);
                        await _ctx.SaveChangesAsync();

                        var role = await _ctx.Roles.FirstOrDefaultAsync(x => x.Id == 1);
                        var userRole = new UserRole
                        {
                            RoleId = role.Id,
                            Role = role,
                            UserId = newUser.Id,
                            User = newUser

                        };
                        
                         newUser.Roles.Add(userRole);
                      var outcome =   await _ctx.SaveChangesAsync();
                        if(outcome > 0)
                        {
                            var bodyMsg = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Verification</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;\">\r\n    <table align=\"center\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd;\">\r\n        <tr>\r\n            <td align=\"center\" style=\"padding: 20px 0;\">\r\n                <h2 style=\"color: #333; margin: 0;\">Email Verification</h2>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 10px 20px; color: #555; font-size: 16px; line-height: 1.5; text-align: center;\">\r\n                <p>Thank you {newUser.Name} for signing up! Please use the OTP code below to verify your email address. This code will expire in 5 minutes : {DateTime.Now.AddMinutes(5).ToString("g")}.</p>\r\n                <p style=\"font-size: 18px; font-weight: bold; color: #007bff; margin: 20px 0; text-align: center;\">\r\n                    <span style=\"background-color: #f0f0f0; padding: 10px 20px; border: 1px solid #ddd; border-radius: 5px; display: inline-block;\">{newUser.VerificationToken}</span>\r\n                </p>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 20px 0; text-align: center; color: #777; font-size: 12px;\">\r\n                <p>If you did not request this, please ignore this email.</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n";

                            var sendMail = new MailRequest
                            {
                                Reciever = newUser.Email,
                                Subject ="Verify your email",
                                Content = bodyMsg
                            };
                            var mail = _emailService.SendEmailAsync(sendMail);
                            response = response.SuccessResult("Account created succesffully, pls kindly verify your email");

                        }
                        else
                        {
                            response = response.FailedResult("Unable to send mail, issue saving to Db");
                        }

                    }
                    else
                    {
                        response = response.FailedResult("Unable to save to database");
                    }
                }
                return response;

            }catch( Exception ex )
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

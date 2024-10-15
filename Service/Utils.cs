using Org.BouncyCastle.Crypto.Generators;
using System.Data.SqlTypes;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace GymMembershipAPI.Service
{
    public static class Utils
    {
        public static string CreateVerificationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(6));
        }

        public static string GenerateAcctNumber()
        {
            Random random = new Random();
            string acctno = "";
            for (int i = 0; i < 6; i++)
            {
                acctno += random.Next(0, 5).ToString();
            }
            return acctno;
        }
        public static string HashPassword(string password)
        {
            if(string.IsNullOrEmpty(password))
            {
                throw new Exception("Password can't be empty");
            }
            else
            {
                 var hashpass = BCrypt.Net.BCrypt.HashPassword(password);
                return hashpass;
            }
        }

        public static bool VerifyHashPasswod(string password,string hashpass)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password can't be empty");
            }
            return BCrypt.Net.BCrypt.Verify(password, hashpass);
        }
    }
}

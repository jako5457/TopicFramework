using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware.Sasl
{
    public class SaslUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public static SaslUser Create(string UserName, string Password)
        {
            using SHA256 sha = SHA256.Create();

            string Salt = GenerateSalt();

            byte[] data = Encoding.UTF8.GetBytes(Password + Salt);

            byte[] Hash = sha.ComputeHash(data);

            return new SaslUser()
            {
                UserName = UserName,
                Salt = Salt,
                Password = BitConverter.ToString(Hash),
            };
        }

        public static string CreateHash(string Password, string Salt)
        {
            using SHA256 sha = SHA256.Create();

            byte[] data = Encoding.UTF8.GetBytes(Password + Salt);

            byte[] Hash = sha.ComputeHash(data);

            return BitConverter.ToString(Hash);
        }

        internal static string GenerateSalt()
        {
            using RandomNumberGenerator rnd = RandomNumberGenerator.Create();

            byte[] bytes = new byte[50];

            rnd.GetNonZeroBytes(bytes);

            return BitConverter.ToString(bytes);
        }
    }
}

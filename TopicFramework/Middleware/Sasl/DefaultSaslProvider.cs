using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware.Sasl
{
    public class DefaultSaslProvider : ISaslProvider
    {

        private List<SaslUser> Users = new List<SaslUser>();

        public DefaultSaslProvider() { }

        public DefaultSaslProvider(params SaslUser[] users) 
        {
            Users.AddRange(users);
        }

        public Task<bool> Authorize(string username, string password,BreakerToken? breakerToken = null)
        {
            var user = Users.Where(u => u.UserName == username).FirstOrDefault();

            if (user == null)
            {
                breakerToken?.Break();
                return Task.FromResult(false);
            }

            var Hash = SaslUser.CreateHash(password, user.Salt);

            if (user.Password == Hash)
            {
                return Task.FromResult(true);
            }

            breakerToken?.Break();
            return Task.FromResult(false);
        }
    }
}

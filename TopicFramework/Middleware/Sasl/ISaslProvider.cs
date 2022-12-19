using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware.Sasl
{
    public interface ISaslProvider
    {

        public Task<bool> Authorize(string username, string password,BreakerToken? breakerToken = null);

        
    }
}

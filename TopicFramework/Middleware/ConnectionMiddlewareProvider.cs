using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Middleware
{
    internal class ConnectionMiddlewareProvider
    {

        List<Action<IServiceProvider,BreakerToken,ConnectionInfo>> _Actions = new();

        private BreakerToken _BreakerToken = new BreakerToken();

        public ConnectionMiddlewareProvider() { }

        public void Add(Action<IServiceProvider, BreakerToken, ConnectionInfo> action) => _Actions.Add(action);

        /// <summary>
        /// executes the middleware
        /// </summary>
        /// <param name="serviceProvider">serviceProvide to be passed</param>
        /// <param name="connectionInfo">Cionnectioninfo to be passed</param>
        /// <returns>False if the circuit has broken</returns>
        public Task<bool> ExecuteAsync(IServiceProvider serviceProvider,ConnectionInfo connectionInfo) 
        {
            foreach (var action in _Actions)
            {
                if (_BreakerToken.IsBroken)
                {
                    return Task.FromResult(false);
                }
                action.Invoke(serviceProvider,_BreakerToken,connectionInfo);
            }
            return Task.FromResult(true);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware.Sasl
{
    public static class SaslExtensions
    {

        public static ConnectionMiddlewareProvider AddSimpleAuthentication(this ConnectionMiddlewareProvider provider)
        {
            provider.Use((s, bt, ci) =>
            {
                var SaslProvider = s.GetRequiredService<ISaslProvider>();
                SaslProvider.Authorize(ci.UserName, ci.Password, bt);
            });
            return provider;
        }

    }
}

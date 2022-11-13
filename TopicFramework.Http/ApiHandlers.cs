using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Http
{
    internal class ApiHandlers
    {
        public static async Task PublishRequest(HttpContext context)
        {
            var scope = context.RequestServices.CreateScope();

            var instance = scope.ServiceProvider.GetRequiredService<TopicInstance>();

            if (!context.Request.HasJsonContentType())
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var msg = Encoding.UTF8.GetBytes("Can only take json.");

                await context.Response.Body.WriteAsync(msg, 0, msg.Length);
            }
            else
            {
                var message = await context.Request.ReadFromJsonAsync<TopicMessage>();

                if (message != null)
                {
                    await instance.SendAsync(message);

                    context.Response.StatusCode = StatusCodes.Status202Accepted;

                    var msg = Encoding.UTF8.GetBytes("Message has been published.");

                    await context.Response.Body.WriteAsync(msg, 0, msg.Length);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var msg = Encoding.UTF8.GetBytes("The json is invalid.");

                    await context.Response.Body.WriteAsync(msg, 0, msg.Length);
                }
            }
            
        }
    }
}

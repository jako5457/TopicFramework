﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Http
{
    public static class HttpExtensions
    {
        
        public static WebApplication AddTopicFrameWorkHttpApi(this WebApplication application,bool Authorize = false)
        {
            application.MapPost("/tf/publish", new RequestDelegate(ApiHandlers.PublishRequest));

            return application;
        }

        public static WebApplication AddTopicFrameWorkHttpApiWithAuthorization(this WebApplication application,params string[] PolicyNames)
        {
            application.MapPost("/tf/publish", new RequestDelegate(ApiHandlers.PublishRequest)).RequireAuthorization(PolicyNames);

            return application;
        }

    }
}

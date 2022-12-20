﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware
{
    public class BrokerConnectionInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public string UserName = string.Empty;

        public string Password = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Connetion
{
    public class ClientConnectionInfo
    {
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public string clientId { get; set; }

        public Dictionary<string, string> Headers { init { _headers = value; } }

        public string GetValue(string key) => _headers[key] ?? null!;
    }
}

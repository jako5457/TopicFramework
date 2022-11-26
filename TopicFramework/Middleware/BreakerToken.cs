using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware
{
    public class BreakerToken
    {

        private bool _Broken = false;

        public BreakerToken() { }

        public bool IsBroken { get { return _Broken; } }

        public void Break() 
        {
            _Broken= true;
        }
    }
}

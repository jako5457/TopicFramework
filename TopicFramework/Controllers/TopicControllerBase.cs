using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Controllers
{
    internal class TopicControllerBase
    {
        protected TopicInstance Instance;
        protected TopicMessage Message;

        public void SetInstance(TopicInstance instance, TopicMessage message)
        {
            Instance = instance;
            Message = message;
        }
    }
}

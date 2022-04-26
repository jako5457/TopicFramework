using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Controllers
{
    /// <summary>
    /// A base class for topiccontrollers
    /// </summary>
    public abstract class TopicControllerBase
    {
        protected TopicInstance Instance;
        protected TopicMessage Message;

        public abstract void OnInitialize(IServiceProvider serviceProvider); 

        public void SetInstance(TopicInstance instance, TopicMessage message)
        {
            Instance = instance;
            Message = message;
        }
    }
}

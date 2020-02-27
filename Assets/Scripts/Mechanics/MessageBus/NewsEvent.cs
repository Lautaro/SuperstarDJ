using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBoardMessaging
{
    public class NewsEvent
    {
        public NewsTopics Topic;
        public object Message;

        public T Open<T>() 
        {
            return (T)Message ;
        }

        public override string ToString()
        {
            return Topic.ToString();

        }
    }
}

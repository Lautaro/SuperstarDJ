using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSystem
{
    public class Message
    {
        public MessageTopics Topic;
        public object Attachment;

        public T Open<T>() 
        {
            return (T)Attachment ;
        }

        public override string ToString()
        {
            return Topic.ToString();

        }
    }
}

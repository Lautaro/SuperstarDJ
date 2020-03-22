using UnityEngine;

namespace SuperstarDJ.MessageSystem
{
    public class Message
    {
        public MessageTopics Topic;
        public object Attachment;

        public T Open<T>()
        {
        
            if ( !(Attachment is T) )
            {
                throw new System.Exception ($"EXCEPTION! A MessageHub message is being cast to wrong type. Correct type : {Attachment.GetType().Name} - Attempted type : {typeof(T).ToString()} - Topic: {Topic}" );
            }
            return ( T )Attachment;
        }

        public override string ToString()
        {
            return Topic.ToString ();

        }
    }
}

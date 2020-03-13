namespace SuperstarDJ.MessageSystem
{
    public class Message
    {
        public MessageTopics Topic;
        public object Attachment;

        public T Open<T>()
        {
            return ( T )Attachment;
        }

        public override string ToString()
        {
            return Topic.ToString ();

        }
    }
}

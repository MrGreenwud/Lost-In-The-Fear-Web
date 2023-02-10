using static BCTSTool.BCTSDebug;

namespace BCTSTool
{
    public class Message
    {
        public string Text { get; private set; }
        public MessageType MassegeType { get; private set; }

        public Message(string messege, MessageType massegeType)
        {
            this.Text = messege;
            this.MassegeType = massegeType;
        }
    }
}

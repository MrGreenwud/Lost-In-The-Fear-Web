using UnityEngine;

namespace BCTSTool
{
    public static class BCTSDebug
    {
        public enum MessageType
        {
            Log,
            Warning,
            Error
        }

        public static string s_Masseges { get; private set; }
        public static int s_MassegesCount { get; private set; }

        private static uint s_MaxMassegeCount = 1000;

        public static void Log(object message)
        {
            if (s_MassegesCount > s_MaxMassegeCount)
                ClearMessege();

            Message newMessage = new Message(message.ToString(), MessageType.Log);

            s_MassegesCount++;
            s_Masseges = s_Masseges + " \n " + newMessage.MassegeType.ToString() + ": " + newMessage.Text;
            Debug.Log(message);
        }

        public static void LogWarning(object message)
        {
            if (s_MassegesCount > s_MaxMassegeCount)
                ClearMessege();

            Message newMessage = new Message(message.ToString(), MessageType.Warning);

            s_MassegesCount++;
            s_Masseges = s_Masseges + " \n " + newMessage.MassegeType.ToString() + ": " + newMessage.Text;
            Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            if (s_MassegesCount > s_MaxMassegeCount)
                ClearMessege();

            Message newMessage = new Message(message.ToString(), MessageType.Error);

            s_MassegesCount++;
            s_Masseges = s_Masseges + " \n " + newMessage.MassegeType.ToString() + ": " + newMessage.Text;
            Debug.LogError(message);
        }

        public static void ClearMessege()
        {
            s_Masseges = null;
            s_MassegesCount = 0;
        }
    }
}

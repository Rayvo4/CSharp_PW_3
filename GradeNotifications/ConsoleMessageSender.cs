using System;

namespace GradeNotifications
{
    // Доставка повідомлень у консоль
    public class ConsoleMessageSender : IMessageSender
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine("   -> [" + recipient + "] " + message);
        }
    }
}

namespace GradeNotifications
{
    // Канал доставки повідомлень (консоль, e-mail, SMS тощо)
    public interface IMessageSender
    {
        void Send(string recipient, string message);
    }
}

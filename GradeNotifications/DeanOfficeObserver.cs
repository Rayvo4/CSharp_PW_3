using System;

namespace GradeNotifications
{
    // Спостерігач-деканат: отримує лише незадовільні оцінки (нижче 60)
    public class DeanOfficeObserver : IGradeObserver
    {
        public const string Recipient = "Деканат";

        private readonly IMessageSender sender;

        // Кількість отриманих сповіщень
        public int NotificationCount { get; private set; }

        public DeanOfficeObserver(IMessageSender sender)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            this.sender = sender;
        }

        public void Update(Grade grade)
        {
            if (grade == null || !grade.IsFailing)
                return;
            NotificationCount++;
            sender.Send(Recipient, "Незадовільна оцінка: " + grade.StudentName + ", «" + grade.Subject + "», " + grade.Score + " балів");
        }
    }
}

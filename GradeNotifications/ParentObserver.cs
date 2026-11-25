using System;

namespace GradeNotifications
{
    // Спостерігач-батьки: отримують оцінки своєї дитини
    public class ParentObserver : IGradeObserver
    {
        private readonly IMessageSender sender;

        public string ParentName { get; private set; }
        public string ChildName { get; private set; }

        // Кількість отриманих сповіщень
        public int NotificationCount { get; private set; }

        public ParentObserver(string parentName, string childName, IMessageSender sender)
        {
            if (string.IsNullOrWhiteSpace(parentName))
                throw new ArgumentException("Ім'я батьків не може бути порожнім", nameof(parentName));
            if (string.IsNullOrWhiteSpace(childName))
                throw new ArgumentException("Ім'я дитини не може бути порожнім", nameof(childName));
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            ParentName = parentName;
            ChildName = childName;
            this.sender = sender;
        }

        public void Update(Grade grade)
        {
            if (grade == null || grade.StudentName != ChildName)
                return;
            NotificationCount++;
            string message = ChildName + " отримав(ла) " + grade.Score + " балів з дисципліни «" + grade.Subject + "»";
            if (grade.IsFailing)
                message += " (незадовільно)";
            sender.Send(ParentName, message);
        }
    }
}

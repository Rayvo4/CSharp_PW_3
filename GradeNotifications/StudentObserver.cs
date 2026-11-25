using System;

namespace GradeNotifications
{
    // Спостерігач-студент: отримує лише власні оцінки
    public class StudentObserver : IGradeObserver
    {
        private readonly IMessageSender sender;

        public string StudentName { get; private set; }

        // Кількість отриманих сповіщень
        public int NotificationCount { get; private set; }

        public StudentObserver(string studentName, IMessageSender sender)
        {
            if (string.IsNullOrWhiteSpace(studentName))
                throw new ArgumentException("Ім'я студента не може бути порожнім", nameof(studentName));
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            StudentName = studentName;
            this.sender = sender;
        }

        public void Update(Grade grade)
        {
            if (grade == null || grade.StudentName != StudentName)
                return;
            NotificationCount++;
            sender.Send(StudentName, "Вам виставлено " + grade.Score + " балів з дисципліни «" + grade.Subject + "»");
        }
    }
}

using System;

namespace GradeNotifications
{
    // Клас даних: оцінка студента з певної дисципліни
    public class Grade
    {
        // Межі шкали оцінювання та прохідний бал
        public const int MinScore = 0;
        public const int MaxScore = 100;
        public const int PassingScore = 60;

        public string StudentName { get; private set; }
        public string Subject { get; private set; }
        public int Score { get; private set; }

        // Чи є оцінка незадовільною
        public bool IsFailing
        {
            get { return Score < PassingScore; }
        }

        public Grade(string studentName, string subject, int score)
        {
            if (string.IsNullOrWhiteSpace(studentName))
                throw new ArgumentException("Ім'я студента не може бути порожнім", nameof(studentName));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Назва дисципліни не може бути порожньою", nameof(subject));
            if (score < MinScore || score > MaxScore)
                throw new ArgumentOutOfRangeException(nameof(score), "Оцінка має бути в межах від 0 до 100");

            StudentName = studentName;
            Subject = subject;
            Score = score;
        }

        public override bool Equals(object obj)
        {
            Grade other = obj as Grade;
            if (other == null)
                return false;
            return StudentName == other.StudentName
                && Subject == other.Subject
                && Score == other.Score;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return StudentName + ", «" + Subject + "»: " + Score;
        }
    }
}

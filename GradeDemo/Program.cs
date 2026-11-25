using System;
using System.Text;
using GradeNotifications;

namespace GradeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Створення журналу (суб'єкта) та спостерігачів
            IMessageSender sender = new ConsoleMessageSender();
            GradeJournal journal = new GradeJournal();
            StudentObserver ivan = new StudentObserver("Іван Петренко", sender);
            StudentObserver maria = new StudentObserver("Марія Коваль", sender);
            ParentObserver ivanMother = new ParentObserver("Олена Петренко", "Іван Петренко", sender);
            DeanOfficeObserver deanOffice = new DeanOfficeObserver(sender);

            Console.WriteLine("=== Підписка спостерігачів на журнал ===");
            journal.Attach(ivan);
            journal.Attach(maria);
            journal.Attach(ivanMother);
            journal.Attach(deanOffice);
            Console.WriteLine("Підписано спостерігачів: " + journal.ObserverCount);

            Console.WriteLine();
            Console.WriteLine("=== Виставлення оцінок ===");
            Grade first = new Grade("Іван Петренко", "Програмування на C#", 92);
            Console.WriteLine("Оцінка: " + first);
            journal.AddGrade(first);

            Grade second = new Grade("Марія Коваль", "ОБД", 55);
            Console.WriteLine("Оцінка: " + second);
            journal.AddGrade(second);

            Grade third = new Grade("Іван Петренко", "Теорія ймовірностей", 48);
            Console.WriteLine("Оцінка: " + third);
            journal.AddGrade(third);

            Console.WriteLine();
            Console.WriteLine("=== Відписка батьків, нова оцінка ===");
            journal.Detach(ivanMother);
            Console.WriteLine("Підписано спостерігачів: " + journal.ObserverCount);
            Grade fourth = new Grade("Іван Петренко", "ОБД", 77);
            Console.WriteLine("Оцінка: " + fourth);
            journal.AddGrade(fourth);

            Console.WriteLine();
            Console.WriteLine("=== Статистика ===");
            Console.WriteLine("Виставлено оцінок: " + journal.GradeCount);
            Console.WriteLine("Остання оцінка: " + journal.LastGrade);
            Console.WriteLine("Сповіщень отримав Іван: " + ivan.NotificationCount);
            Console.WriteLine("Сповіщень отримала Марія: " + maria.NotificationCount);
            Console.WriteLine("Сповіщень отримала Олена Петренко: " + ivanMother.NotificationCount);
            Console.WriteLine("Сповіщень отримав деканат: " + deanOffice.NotificationCount);

            Console.WriteLine();
            Console.WriteLine("=== Некоректна оцінка ===");
            try
            {
                journal.AddGrade(new Grade("Іван Петренко", "ОБД", 120));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Помилка: " + e.Message);
            }

            Console.ReadKey();
        }
    }
}

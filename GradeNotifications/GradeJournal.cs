using System;
using System.Collections.Generic;

namespace GradeNotifications
{
    // Конкретний суб'єкт: журнал оцінок
    public class GradeJournal : IGradeSubject
    {
        private readonly List<IGradeObserver> observers = new List<IGradeObserver>();
        private readonly List<Grade> grades = new List<Grade>();

        // Кількість підписаних спостерігачів
        public int ObserverCount
        {
            get { return observers.Count; }
        }

        // Кількість виставлених оцінок
        public int GradeCount
        {
            get { return grades.Count; }
        }

        // Остання виставлена оцінка
        public Grade LastGrade { get; private set; }

        public void Attach(IGradeObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observers.Contains(observer))
                throw new InvalidOperationException("Спостерігач вже підписаний на журнал");
            observers.Add(observer);
        }

        public void Detach(IGradeObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            observers.Remove(observer);
        }

        // Сповіщення всіх підписників; перебір копії списку дозволяє
        // спостерігачу відписатися під час обробки сповіщення
        public void Notify(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException(nameof(grade));
            foreach (IGradeObserver observer in observers.ToArray())
                observer.Update(grade);
        }

        // Виставлення оцінки: зберігає її та сповіщає підписників
        public void AddGrade(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException(nameof(grade));
            grades.Add(grade);
            LastGrade = grade;
            Notify(grade);
        }
    }
}

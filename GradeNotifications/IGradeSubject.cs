namespace GradeNotifications
{
    // Суб'єкт (Subject): керує підпискою та сповіщає спостерігачів
    public interface IGradeSubject
    {
        void Attach(IGradeObserver observer);
        void Detach(IGradeObserver observer);
        void Notify(Grade grade);
    }
}

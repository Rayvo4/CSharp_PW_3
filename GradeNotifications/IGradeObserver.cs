namespace GradeNotifications
{
    // Спостерігач (Observer): отримує сповіщення про нову оцінку
    public interface IGradeObserver
    {
        void Update(Grade grade);
    }
}

namespace MyCalendar.Notification
{
    public interface INotificationManager
    {
        void Remind(long dateTime, string title, string message);
    }
}

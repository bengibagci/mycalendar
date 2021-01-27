using System;

namespace MyCalendar.Notification
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public long DateTime { get; set; }
    }
}

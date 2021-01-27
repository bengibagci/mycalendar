using Android.App;
using Android.Content;
using Android.OS;
using MyCalendar.Notification;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(MyCalendar.Droid.Notification.AndroidNotificationManager))]
namespace MyCalendar.Droid.Notification
{
    public class AndroidNotificationManager : INotificationManager
    {
        public const string TitleKey = "title";
        public const string MessageKey = "message";

        [Obsolete]
        public void Remind(long dateTime, string title, string message)
        {
            Intent alarmIntent = new Intent(AndroidApp.Context, typeof(AndroidNotificationReceiver));
            alarmIntent.PutExtra("title", title);
            alarmIntent.PutExtra("message", message);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            AlarmManager alarmManager = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {

                alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + dateTime, pendingIntent);
            }
            else
            {

                alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + dateTime, pendingIntent);
            }
        }
    }
}
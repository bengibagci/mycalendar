using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using System;

namespace MyCalendar.Droid.Notification
{
    [Obsolete]
    [BroadcastReceiver(Enabled = true, Exported = true, Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class AndroidNotificationReceiver : BroadcastReceiver
    {
        private const string channelId = "default";
        private const string channelName = "Default";
        private const string channelDescription = "The default channel for notifications.";
        private bool channelInitialized = false;
        private NotificationManager manager;

        public override void OnReceive(Context context, Intent intent)
        {
            if ("android.intent.action.BOOT_COMPLETED".Equals(intent.Action))
            {
                Intent serviceIntent = new Intent("com.companyname.mycalendar");
                context.StartService(serviceIntent);
            }

            manager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Java.Lang.String channelNameJava = new Java.Lang.String(channelName);
                NotificationChannel channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;

            string message = intent.GetStringExtra("message");
            string title = intent.GetStringExtra("title");

            Intent resultIntent = new Intent(context, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.CancelCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Mipmap.icon))
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            manager.Notify(1, builder.Build());
        }
    }
}
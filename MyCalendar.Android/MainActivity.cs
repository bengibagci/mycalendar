using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using MyCalendar.Droid.Notification;

namespace MyCalendar.Droid
{
    [Activity(Label = "MyCalendar", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private AndroidNotificationReceiver receiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            receiver = new AndroidNotificationReceiver();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CachedImageRenderer.Init(true);
            FlowListView.Init();
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            XamForms.Controls.Droid.Calendar.Init();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            RegisterReceiver(receiver, new IntentFilter("com.companyname.mycalendar"));
        }

        protected override void OnPause()
        {
            UnregisterReceiver(receiver);

            base.OnPause();
        }
    }
}
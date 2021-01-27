using MyCalendar.Views;
using Xamarin.Forms;

namespace MyCalendar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CalendarPage())
            {
                BarBackgroundColor = Color.FromHex(Resx.ColorResource.FOBlue),
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

using MyCalendar.Models;
using MyCalendar.Notification;
using MyCalendar.Services;
using MyCalendar.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateReminderPage : ContentPage
    {
        private readonly INotificationManager notificationManager;
        private readonly CreateReminderVM viewModel;
        public CreateReminderPage(string type, Reminder r)
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            IReminderService reminderService = new ReminderService(DependencyService.Get<ISQLiteDb>());

            viewModel = new CreateReminderVM(new PageService(), type, reminderService, r, notificationManager);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SelectedDay();
            SelectedTime();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<DatepickerPopup, (string, string)>(this, "SelectedDay");
            MessagingCenter.Unsubscribe<TimepickerPopup, (string, string)>(this, "SelectedTime");
        }

        private void SelectedDay()
        {
            MessagingCenter.Subscribe<DatepickerPopup, (string, string)>(this, "SelectedDay", (sender, args) =>
            {
                string autoId = args.Item1 as string;
                string date = args.Item2 as string;

                if (autoId == "ReminderDay")
                {
                    viewModel.ReminderDay = date;
                }
            });
        }

        private void SelectedTime()
        {
            MessagingCenter.Subscribe<TimepickerPopup, (string, string)>(this, "SelectedTime", (sender, args) =>
            {
                string autoId = args.Item1 as string;
                string time = args.Item2 as string;

                if (autoId == "ReminderTime")
                {
                    viewModel.ReminderTime = time;
                }
            });
        }

        [Obsolete]
        private async void OnCalendar(object sender, EventArgs e)
        {
            Frame s = sender as Frame;
            string autoId = s.AutomationId;
            await PopupNavigation.PushAsync(new DatepickerPopup(autoId)).ConfigureAwait(false);
        }

        [Obsolete]
        private async void OnHour(object sender, EventArgs e)
        {
            Frame s = sender as Frame;
            string autoId = s.AutomationId;
            await PopupNavigation.PushAsync(new TimepickerPopup(autoId)).ConfigureAwait(false);
        }
    }
}
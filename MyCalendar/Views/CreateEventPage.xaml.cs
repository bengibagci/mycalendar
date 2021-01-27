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
    public partial class CreateEventPage : ContentPage
    {
        private readonly INotificationManager notificationManager;
        private readonly CreateEventVM viewModel;

        [Obsolete]
        public CreateEventPage(string type, Event e)
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            IEventService eventService = new EventService(DependencyService.Get<ISQLiteDb>());

            viewModel = new CreateEventVM(new PageService(), type, eventService, e, notificationManager);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SelectedNotice();
            SelectedDay();
            SelectedTime();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<OptionsPopupVM, PopupItem>(this, "SelectedNotice");
            MessagingCenter.Unsubscribe<DatepickerPopup, (string, string)>(this, "SelectedDay");
            MessagingCenter.Unsubscribe<TimepickerPopup, (string, string)>(this, "SelectedTime");
        }

        private void SelectedNotice()
        {
            MessagingCenter.Subscribe<OptionsPopupVM, PopupItem>(this, "SelectedNotice", (sender, args) =>
            {
                PopupItem e = args as PopupItem;
                if (e != null)
                {
                    viewModel.Notice = e.Id;
                    viewModel.NoticeTitle = e.Title;
                }
            });
        }

        private void SelectedDay()
        {
            MessagingCenter.Subscribe<DatepickerPopup, (string, string)>(this, "SelectedDay", (sender, args) =>
            {
                string autoId = args.Item1 as string;
                string date = args.Item2 as string;

                if (autoId == "StartDay")
                {
                    viewModel.StartDay = date;
                }
                else
                {
                    viewModel.EndDay = date;
                }
            });
        }

        private void SelectedTime()
        {
            MessagingCenter.Subscribe<TimepickerPopup, (string, string)>(this, "SelectedTime", (sender, args) =>
            {
                string autoId = args.Item1 as string;
                string time = args.Item2 as string;

                if (autoId == "StartTime")
                {
                    viewModel.StartTime = time;
                }
                else
                {
                    viewModel.EndTime = time;
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

        private void IsFullDayToggled(object sender, ToggledEventArgs e)
        {
            viewModel.IsFullDayToggledCommand.Execute(e);
        }

        private void IsOneDayToggled(object sender, ToggledEventArgs e)
        {
            viewModel.IsOneDayToggledCommand.Execute(e);
        }
    }
}
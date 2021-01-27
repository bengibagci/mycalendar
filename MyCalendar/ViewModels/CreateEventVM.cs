using MyCalendar.Enums;
using MyCalendar.Models;
using MyCalendar.Notification;
using MyCalendar.Services;
using MyCalendar.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class CreateEventVM : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IPageService _pageService;
        private readonly INotificationManager _notificationManager;

        private int notificationNumber = 0;

        public Event Event { get; private set; }
        public ICommand OnSaveCommand { get; private set; }
        public ICommand SelectNoticeCommand { get; private set; }
        public ICommand IsFullDayToggledCommand { get; private set; }
        public ICommand IsOneDayToggledCommand { get; private set; }

        [Obsolete]
        public CreateEventVM(IPageService pageService, string type, IEventService eventService, Event e, INotificationManager notificationManager)
        {
            _pageService = pageService;
            _eventService = eventService;
            _notificationManager = notificationManager;

            if (type == "create")
            {
                PageTitle = "Create Event";

                Notice = Notice.None;
                NoticeTitle = Notice.ToString();
                StartDay = DateTime.Today.ToString("dd/MM/yyyy");
                StartTime = DateTime.Now.ToString("HH:mm");
                EndDay = DateTime.Today.ToString("dd/MM/yyyy");
                EndTime = DateTime.Now.ToString("HH:mm");
                IsFullDay = false;
                IsEnableStartTime = true;
                IsEnableEndTime = true;
                IsOneDay = false;
                IsEnableOneDay = true;
            }
            else
            {
                PageTitle = "Edit Event";

                Event = e;

                Title = e.Title;
                Description = e.Description;
                StartDay = e.StartDate.ToString("dd/MM/yyyy");
                StartTime = e.StartDate.ToString("HH:mm");
                EndDay = e.EndDate.ToString("dd/MM/yyyy");
                EndTime = e.EndDate.ToString("HH:mm");
                Notice = e.Notice;
                IsFullDay = e.IsFullDay;
                IsOneDay = e.IsOneDay;

                IsEnableStartTime = !e.IsFullDay;
                IsEnableEndTime = !e.IsOneDay;
                IsEnableOneDay = !e.IsOneDay;
            }

            OnSaveCommand = new Command(() => OnSave(type));
            SelectNoticeCommand = new Command(() => SelectNotice());
            IsFullDayToggledCommand = new Command(() => IsFullDayToggled());
            IsOneDayToggledCommand = new Command(() => IsOneDayToggled());
        }

        private void OnSave(string type)
        {
            string _startDate = @StartDay + @" " + @StartTime;
            DateTime startDate = Convert.ToDateTime(_startDate, System.Globalization.CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat);

            string _endDate = @EndDay + @" " + @EndTime;
            DateTime endDate = Convert.ToDateTime(_endDate, System.Globalization.CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat);

            if (type == "create")
            {
                Event = new Event
                {
                    Title = Title,
                    Description = Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Notice = Notice,
                    IsFullDay = IsFullDay,
                    IsOneDay = IsOneDay,
                    IsEvent = true,
                    CreatedDate = DateTime.Now,
                    UpdateDate = null,
                    IconPath = "dot.png"
                };
                _eventService.AddEvent(Event);

                CalendarPageVM.IsAddedEvent = false;
            }
            else
            {
                Event.Title = Title;
                Event.Description = Description;
                Event.StartDate = startDate;
                Event.EndDate = endDate;
                Event.Notice = Notice;
                Event.IsFullDay = IsFullDay;
                Event.IsOneDay = IsOneDay;
                Event.UpdateDate = DateTime.Now;

                _eventService.UpdateEvent(Event);

                CalendarPageVM.IsUpdatedEvent = false;
                EventListVM.IsUpdatedEvent = false;
            }

            Thread.Sleep(500);
            _pageService.PopToRootAsync();

            Notification(Event.Title, Event.StartDate, Event.Notice);
        }

        private void Notification(string eventTitle, DateTime date, Notice notice)
        {
            DateTime eventDate;
            if (notice == Notice.FiveMinutes)
            {
                eventDate = date.AddMinutes(5);
            }
            else if (notice == Notice.TenMinutes)
            {
                eventDate = date.AddMinutes(10);
            }
            else if (notice == Notice.FifteenMinutes)
            {
                eventDate = date.AddMinutes(15);
            }
            else if (notice == Notice.ThirtyMinutes)
            {
                eventDate = date.AddMinutes(30);
            }
            else if (notice == Notice.OneHour)
            {
                eventDate = date.AddHours(1);
            }
            else if (notice == Notice.TwoHours)
            {
                eventDate = date.AddMinutes(2);
            }
            else if (notice == Notice.OneDay)
            {
                eventDate = date.AddDays(1);
            }
            else if (notice == Notice.TwoDays)
            {
                eventDate = date.AddDays(2);
            }
            else
            {
                eventDate = date;
            }

            notificationNumber++;
            string title = $"Event";
            string message = $"It's time for " + eventTitle;
            long time = (long)(eventDate - DateTime.Now).TotalMilliseconds;
            _notificationManager.Remind(time, title, message);
        }

        [Obsolete]
        private async void SelectNotice()
        {
            await PopupNavigation.PushAsync(new OptionsPopup()).ConfigureAwait(false);
        }

        private void IsFullDayToggled()
        {
            if (IsFullDay == true)
            {
                IsSwitchOneEnable = false;
                IsEnableStartTime = false;
                IsEnableEndTime = false;

                StartTime = "00:00";
                EndTime = "23:59";
            }
            else
            {
                IsSwitchOneEnable = true;
                IsEnableStartTime = true;
                IsEnableEndTime = true;
            }
        }

        private void IsOneDayToggled()
        {
            if (IsOneDay == true)
            {
                IsSwitchFullEnable = false;
                IsEnableEndTime = false;

                string currentDate = @StartDay;
                DateTime date = Convert.ToDateTime(currentDate, System.Globalization.CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat);
                EndDay = date.AddDays(1).ToString("dd/MM/yyyy");
                EndTime = StartTime;
            }
            else
            {
                IsSwitchFullEnable = true;
                IsEnableEndTime = false;
            }
        }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetValue(ref _pageTitle, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value);
        }

        private string _startDay;
        public string StartDay
        {
            get => _startDay;
            set => SetValue(ref _startDay, value);
        }

        private string _startTime;
        public string StartTime
        {
            get => _startTime;
            set => SetValue(ref _startTime, value);
        }

        private string _endDay;
        public string EndDay
        {
            get => _endDay;
            set => SetValue(ref _endDay, value);
        }

        private string _endTime;
        public string EndTime
        {
            get => _endTime;
            set => SetValue(ref _endTime, value);
        }

        private bool _isFullDay;
        public bool IsFullDay
        {
            get => _isFullDay;
            set => SetValue(ref _isFullDay, value);
        }

        private Notice _notice;
        public Notice Notice
        {
            get => _notice;
            set => SetValue(ref _notice, value);
        }

        private string _noticeTitle;
        public string NoticeTitle
        {
            get => _noticeTitle;
            set => SetValue(ref _noticeTitle, value);
        }

        private bool _isEnableEndTime;
        public bool IsEnableEndTime
        {
            get => _isEnableEndTime;
            set => SetValue(ref _isEnableEndTime, value);
        }

        private bool _isEnableStartTime;
        public bool IsEnableStartTime
        {
            get => _isEnableStartTime;
            set => SetValue(ref _isEnableStartTime, value);
        }

        private bool _isOneDay;
        public bool IsOneDay
        {
            get => _isOneDay;
            set => SetValue(ref _isOneDay, value);
        }

        private bool _isEnableOneDay;
        public bool IsEnableOneDay
        {
            get => _isEnableOneDay;
            set => SetValue(ref _isEnableOneDay, value);
        }

        private bool _isSwitchFullEnable = true;
        public bool IsSwitchFullEnable
        {
            get => _isSwitchFullEnable;
            set => SetValue(ref _isSwitchFullEnable, value);
        }

        private bool _isSwitchOneEnable = true;
        public bool IsSwitchOneEnable
        {
            get => _isSwitchOneEnable;
            set => SetValue(ref _isSwitchOneEnable, value);
        }
    }
}

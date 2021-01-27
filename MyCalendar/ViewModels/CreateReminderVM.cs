using MyCalendar.Enums;
using MyCalendar.Models;
using MyCalendar.Notification;
using MyCalendar.Services;
using System;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class CreateReminderVM : BaseViewModel
    {
        private readonly IReminderService _reminderService;
        private readonly IPageService _pageService;
        private readonly INotificationManager _notificationManager;

        private int notificationNumber = 0;

        public Reminder Reminder { get; private set; }
        public ICommand SetNoneVisibilityCommand => new Command(() => SetNoneVisibility());
        public ICommand SetLowVisibilityCommand => new Command(() => SetLowVisibility());
        public ICommand SetMediumVisibilityCommand => new Command(() => SetMediumVisibility());
        public ICommand SetHighVisibilityCommand => new Command(() => SetHighVisibility());
        public ICommand OnSaveCommand { get; private set; }
        public CreateReminderVM(IPageService pageSevice, string type, IReminderService reminderService, Reminder r, INotificationManager notificationManager)
        {
            _pageService = pageSevice;
            _reminderService = reminderService;
            _notificationManager = notificationManager;

            if (type == "create")
            {
                PageTitle = "Create Reminder";

                NoneVisiblity = true;
                LowVisiblity = false;
                MediumVisiblity = false;
                HighVisiblity = false;
                ReminderDay = DateTime.Today.ToString("dd/MM/yyyy");
                ReminderTime = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                PageTitle = "Edit Reminder";

                Reminder = r;

                Title = r.Title;
                Note = r.Note;
                ReminderDay = r.ReminderDate.ToString("dd/MM/yyyy");
                ReminderTime = r.ReminderDate.ToString("HH:mm");
                Priority = r.Priority;

                if (Priority == Priority.High)
                {
                    NoneVisiblity = false;
                    LowVisiblity = false;
                    MediumVisiblity = false;
                    HighVisiblity = true;
                }
                else if (Priority == Priority.Medium)
                {
                    NoneVisiblity = false;
                    LowVisiblity = false;
                    MediumVisiblity = true;
                    HighVisiblity = false;
                }
                else if (Priority == Priority.Low)
                {
                    NoneVisiblity = false;
                    LowVisiblity = true;
                    MediumVisiblity = false;
                    HighVisiblity = false;
                }
                else
                {
                    NoneVisiblity = true;
                    LowVisiblity = false;
                    MediumVisiblity = false;
                    HighVisiblity = false;
                }
            }

            OnSaveCommand = new Command(() => OnSave(type));
        }

        [Obsolete]
        private void OnSave(string type)
        {
            string _reminderDate = @ReminderDay + @" " + @ReminderTime;
            DateTime reminderDate = Convert.ToDateTime(_reminderDate, System.Globalization.CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat);


            if (type == "create")
            {
                Reminder = new Reminder
                {
                    Title = Title,
                    Note = Note,
                    ReminderDate = reminderDate,
                    Priority = Priority,
                    IsEvent = false,
                    CreatedDate = DateTime.Now,
                    UpdateDate = null,
                };

                if (Reminder.Priority == Priority.High)
                    Reminder.IconPath = "priorityRed.png";
                else if(Reminder.Priority == Priority.Medium)
                    Reminder.IconPath = "priorityYellow.png";
                else if (Reminder.Priority == Priority.Low)
                    Reminder.IconPath = "priorityGreen.png";
                else
                    Reminder.IconPath = "dot.png";

                _reminderService.AddReminder(Reminder);

                CalendarPageVM.IsAddedReminder = false;
            }
            else
            {
                Reminder.Title = Title;
                Reminder.Note = Note;
                Reminder.ReminderDate = reminderDate;
                Reminder.Priority = Priority;
                Reminder.UpdateDate = DateTime.Now;

                if (Reminder.Priority == Priority.High)
                    Reminder.IconPath = "priorityRed.png";
                else if (Reminder.Priority == Priority.Medium)
                    Reminder.IconPath = "priorityYellow.png";
                else if (Reminder.Priority == Priority.Low)
                    Reminder.IconPath = "priorityGreen.png";
                else
                    Reminder.IconPath = "dot.png";

                _reminderService.UpdateReminder(Reminder);

                CalendarPageVM.IsUpdatedReminder = false;
                EventListVM.IsUpdatedReminder = false;
            }

            Thread.Sleep(500);
            _pageService.PopToRootAsync();

            Notification(Reminder.Title, Reminder.ReminderDate);
        }

        private void Notification(string reminderTitle, DateTime date)
        {
            notificationNumber++;
            string title = $"Reminder";
            string message = $"It's time for " + reminderTitle;
            long time = (long)(date - DateTime.Now).TotalMilliseconds;

            _notificationManager.Remind(time, title, message);
        }

        private void SetNoneVisibility()
        {
            if (NoneVisiblity == false)
            {
                NoneVisiblity = true;
                LowVisiblity = false;
                MediumVisiblity = false;
                HighVisiblity = false;
            }

            Priority = Priority.None;
        }

        private void SetLowVisibility()
        {
            if (LowVisiblity == false)
            {
                NoneVisiblity = false;
                LowVisiblity = true;
                MediumVisiblity = false;
                HighVisiblity = false;
            }

            Priority = Priority.Low;
        }

        private void SetMediumVisibility()
        {
            if (MediumVisiblity == false)
            {
                NoneVisiblity = false;
                LowVisiblity = false;
                MediumVisiblity = true;
                HighVisiblity = false;
            }

            Priority = Priority.Medium;
        }

        private void SetHighVisibility()
        {
            if (HighVisiblity == false)
            {
                NoneVisiblity = false;
                LowVisiblity = false;
                MediumVisiblity = false;
                HighVisiblity = true;
            }

            Priority = Priority.High;
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

        private string _note;
        public string Note
        {
            get => _note;
            set => SetValue(ref _note, value);
        }

        private string _reminderDay;
        public string ReminderDay
        {
            get => _reminderDay;
            set => SetValue(ref _reminderDay, value);
        }

        private string _reminderTime;
        public string ReminderTime
        {
            get => _reminderTime;
            set => SetValue(ref _reminderTime, value);
        }

        private Priority _priority;
        public Priority Priority
        {
            get => _priority;
            set => SetValue(ref _priority, value);
        }

        private bool _noneVisiblity;
        public bool NoneVisiblity
        {
            get => _noneVisiblity;
            set => SetValue(ref _noneVisiblity, value);
        }

        private bool _lowVisiblity;
        public bool LowVisiblity
        {
            get => _lowVisiblity;
            set => SetValue(ref _lowVisiblity, value);
        }

        private bool _mediumVisiblity;
        public bool MediumVisiblity
        {
            get => _mediumVisiblity;
            set => SetValue(ref _mediumVisiblity, value);
        }

        private bool _highVisiblity;
        public bool HighVisiblity
        {
            get => _highVisiblity;
            set => SetValue(ref _highVisiblity, value);
        }
    }
}

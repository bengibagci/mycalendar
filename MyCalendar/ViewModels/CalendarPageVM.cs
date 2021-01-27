using MyCalendar.Models;
using MyCalendar.Services;
using MyCalendar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamForms.Controls;

namespace MyCalendar.ViewModels
{
    public class CalendarPageVM : BaseViewModel
    {
        public static bool IsAddedEvent = true;
        public static bool IsAddedReminder = true;
        public static bool IsUpdatedEvent = true;
        public static bool IsUpdatedReminder = true;
        public static bool IsDeleted = true;

        private bool _isDataLoaded;
        private bool _isRefresh = false;

        private readonly IPageService _pageService;
        private readonly IEventService _eventService;
        private readonly IReminderService _reminderService;

        private List<Reminder> GetReminders { get; set; }
        private List<Event> GetEvents { get; set; }
        public static List<DateTime> TabDates { get; private set; } = new List<DateTime>();
        public ObservableCollection<object> DataList { get; private set; } = new ObservableCollection<object>();

        public ICommand LoadDataCommand { get; private set; }
        public ICommand DateChosenCommand { get; private set; }
        public ICommand TappedEventCommand { get; private set; }
        public ICommand ClickFabCommand { get; private set; }
        public ICommand CreateEventCommand { get; private set; }
        public ICommand CreateReminderCommand { get; private set; }
        public ICommand OnListCommand { get; private set; }
        public ICommand RefreshAttendancesCommand { get; private set; }
        public ICommand RefreshCalendarCommand { get; private set; }

        [Obsolete]
        public CalendarPageVM(IPageService pageService, IEventService eventService, IReminderService reminderService)
        {
            _pageService = pageService;
            _eventService = eventService;
            _reminderService = reminderService;

            EventVisiblity = false;
            ReminderVisiblity = false;

            SelectedDateText = SelectedDate.ToString("dddd, dd MMMM yyyy");

            TabDates.Add(SelectedDate);

            dates = new List<SpecialDate>();

            Attendances = new ObservableCollection<SpecialDate>(dates)
            {
                new SpecialDate(DateTime.Today)
                {
                    BackgroundColor = Color.FromHex("Resx.ColorResource.FillColor"),
                    BorderColor = Color.FromHex("Resx.ColorResource.BorderColor"),
                    TextColor = Color.White,
                    Selectable = true
                },
            };

            LoadDataCommand = new Command(() => LoadData());
            DateChosenCommand = new Command((obj) => DateChosen(obj));
            ClickFabCommand = new Command(() => ClickFab());
            CreateEventCommand = new Command(async () => await CreateEvent().ConfigureAwait(false));
            CreateReminderCommand = new Command(async () => await CreateReminder().ConfigureAwait(false));
            TappedEventCommand = new Command(async (param) => await TappedEvent(param).ConfigureAwait(false));
            OnListCommand = new Command(() => OnList());
            RefreshAttendancesCommand = new Command(() => RefreshAttendances());
            RefreshCalendarCommand = new Command(() => RefreshCalendar());

            GetAttendances();
        }

        private async void GetAttendances()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            if (GetEvents.Any())
            {
                foreach (Event e in GetEvents)
                {
                    DateTime startDate = e.StartDate.Date;
                    DateTime endDate = e.EndDate.Date;
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1.0))
                    {
                        SpecialDate eventDate = new SpecialDate(date)
                        {
                            BorderColor = Color.OrangeRed,
                            BorderWidth = 2,
                            Selectable = true
                        };

                        if (!Attendances.Contains(eventDate))
                        {
                            Attendances.Add(eventDate);
                        }
                    }
                }
            }

            if (GetReminders.Any())
            {
                foreach (Reminder r in GetReminders)
                {
                    SpecialDate reminderDate = new SpecialDate(r.ReminderDate.Date)
                    {
                        BorderColor = Color.OrangeRed,
                        BorderWidth = 2,
                        Selectable = true
                    };

                    if (!Attendances.Contains(reminderDate))
                    {
                        Attendances.Add(reminderDate);
                    }
                }
            }
        }

        private async void RefreshAttendances()
        {
            if (_isRefresh == false)
                return;

            Attendances.Clear();
            Attendances = new ObservableCollection<SpecialDate>(dates)
            {
                new SpecialDate(DateTime.Today)
                {
                    BackgroundColor = Color.FromHex("Resx.ColorResource.FillColor"),
                    BorderColor = Color.FromHex("Resx.ColorResource.BorderColor"),
                    TextColor = Color.White,
                    Selectable = true
                },
            };
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            if (GetEvents.Any())
            {
                foreach (Event e in GetEvents)
                {
                    DateTime startDate = e.StartDate.Date;
                    DateTime endDate = e.EndDate.Date;
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1.0))
                    {
                        SpecialDate eventDate = new SpecialDate(date)
                        {
                            BorderColor = Color.OrangeRed,
                            BorderWidth = 2,
                            Selectable = true
                        };

                        if (!Attendances.Contains(eventDate))
                        {
                            Attendances.Add(eventDate);
                        }
                    }
                }
            }

            if (GetReminders.Any())
            {
                foreach (Reminder r in GetReminders)
                {
                    SpecialDate reminderDate = new SpecialDate(r.ReminderDate.Date)
                    {
                        BorderColor = Color.OrangeRed,
                        BorderWidth = 2,
                        Selectable = true
                    };

                    if (!Attendances.Contains(reminderDate))
                    {
                        Attendances.Add(reminderDate);
                    }
                }
            }
        }

        private async void LoadData()
        {
            if (_isDataLoaded)
            {
                if (IsAddedEvent == false)
                {
                    AddEvent();
                }

                if (IsAddedReminder == false)
                {
                    AddReminder();
                }

                if (IsUpdatedEvent == false)
                {
                    UpdateEvent();
                }

                if (IsUpdatedReminder == false)
                {
                    UpdateReminder();
                }

                if (IsDeleted == false)
                {
                    Delete();
                }

                return;
            }

            _isDataLoaded = true;

            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            SelectedData(SelectedDate);
        }

        private ObservableCollection<object> SelectedData(DateTime? date)
        {
            if (GetEvents.Any())
            {
                foreach (Event e in GetEvents)
                {
                    DateTime startDate = e.StartDate.Date.AddDays(-1);
                    DateTime finalDate = e.EndDate.Date.AddDays(1);
                    if (date < finalDate && date > startDate)
                    {
                        DataList.Add(e);
                    }
                }
            }

            if (GetReminders.Any())
            {
                foreach (Reminder r in GetReminders)
                {
                    if (r.ReminderDate.Date == date)
                    {
                        DataList.Add(r);
                    }
                }
            }

            Thread.Sleep(1000);
            return DataList;
        }

        private async Task<ObservableCollection<object>> AddEvent()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);

            if (GetEvents.Any())
            {
                Event newEvent = GetEvents.OrderBy(x => x.CreatedDate).Last();

                if (!DataList.Contains(newEvent))
                {
                    DateTime _date = DateTime.Today.Date;
                    DateTime startDate = newEvent.StartDate.Date.AddDays(-1);
                    DateTime finalDate = newEvent.EndDate.Date.AddDays(1);

                    if (_date < finalDate && _date > startDate)
                    {
                        DataList.Add(newEvent);
                    }
                }

                for (DateTime d = newEvent.StartDate.Date; d <= newEvent.EndDate.Date; d = d.AddDays(1.0))
                {
                    SpecialDate eventDate = new SpecialDate(d)
                    {
                        BorderColor = Color.OrangeRed,
                        BorderWidth = 2,
                        Selectable = true
                    };

                    if (!Attendances.Contains(eventDate))
                    {
                        Attendances.Add(eventDate);
                    }
                }
            }

            IsAddedEvent = true;
            _isRefresh = true;

            Thread.Sleep(1000);
            return DataList;
        }

        private async Task<ObservableCollection<object>> AddReminder()
        {
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            if (GetReminders.Any())
            {
                Reminder newReminder = GetReminders.OrderBy(x => x.CreatedDate).Last();

                if (!DataList.Contains(newReminder))
                {
                    DateTime date = DateTime.Today.Date;

                    if (date.Date == newReminder.ReminderDate.Date)
                    {
                        DataList.Add(newReminder);
                    }
                }

                SpecialDate reminderDate = new SpecialDate(newReminder.ReminderDate.Date)
                {
                    BorderColor = Color.OrangeRed,
                    BorderWidth = 2,
                    Selectable = true
                };

                if (!Attendances.Contains(reminderDate))
                {
                    Attendances.Add(reminderDate);
                }
            }

            IsAddedReminder = true;
            _isRefresh = true;

            Thread.Sleep(1000);
            return DataList;
        }

        private async Task<ObservableCollection<object>> UpdateEvent()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);

            if (GetEvents.Any())
            {
                Event updatedEvent = GetEvents.Where(x => x.UpdateDate != null).OrderBy(x => x.UpdateDate).Last();

                foreach (object i in DataList)
                {
                    if (i is Event item)
                    {
                        if (item.Id == updatedEvent.Id)
                        {
                            DataList.Remove(item);
                            DataList.Add(updatedEvent);

                            break;
                        }
                    }
                }
            }

            IsUpdatedEvent = true;
            _isRefresh = true;

            Thread.Sleep(1000);
            return DataList;
        }

        private async Task<ObservableCollection<object>> UpdateReminder()
        {
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            if (GetReminders.Any())
            {
                Reminder updatedReminder = GetReminders.Where(x => x.UpdateDate != null).OrderBy(x => x.UpdateDate).Last();

                foreach (object i in DataList)
                {
                    if (i is Reminder item)
                    {
                        if (item.Id == updatedReminder.Id)
                        {
                            DataList.Remove(item);
                            DataList.Add(updatedReminder);

                            break;
                        }
                    }
                }
            }

            IsUpdatedReminder = true;
            _isRefresh = true;

            Thread.Sleep(1000);
            return DataList;
        }

        private async Task<ObservableCollection<object>> Delete()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            int count = 0;
            while (count < DataList.Count)
            {
                if (DataList[count] is Event eventModel)
                {
                    if (!GetEvents.Any(x => x.Id == eventModel.Id))
                    {
                        DataList.Remove(eventModel);

                        DateTime startDate = eventModel.StartDate.Date;
                        DateTime endDate = eventModel.EndDate.Date;
                        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1.0))
                        {
                            SpecialDate ed = new SpecialDate(date)
                            {
                                BorderColor = Color.OrangeRed,
                                BorderWidth = 2,
                                Selectable = true
                            };

                            Attendances.Remove(ed);
                        }
                    }
                }
                else if (DataList[count] is Reminder reminderModel)
                {
                    if (!GetReminders.Any(x => x.Id == reminderModel.Id))
                    {
                        DataList.Remove(reminderModel);

                        SpecialDate rd = new SpecialDate(reminderModel.ReminderDate.Date)
                        {
                            BorderColor = Color.OrangeRed,
                            BorderWidth = 2,
                            Selectable = true
                        };
                        Attendances.Remove(rd);
                    }
                }

                count++;
            }

            IsDeleted = true;
            _isRefresh = true;

            Thread.Sleep(1000);
            return DataList;
        }

        private void DateChosen(object obj)
        {
            DateTime? d = obj as DateTime?;
            DateTime date = (DateTime)d;

            if (TabDates.Contains(date))
            {
                TabDates.Remove(date);
            }
            else
            {
                TabDates.Add(date);
            }

            if (TabDates.Count < 2)
            {
                DateTime first;
                if (TabDates.Count == 0)
                {
                    first = DateTime.Today;
                }
                else
                {
                    first = TabDates.OrderBy(x => x).First();
                }

                SelectedDateText = first.ToString("dddd, dd MMMM yyyy");
                IsMultiselect = false;
                DataList.Clear();
                SelectedData(first);
            }
            else
            {
                DateTime start = TabDates.OrderBy(x => x).First();
                DateTime end = TabDates.OrderBy(x => x).Last();
                SelectedDateText = start.ToString("dd MMMM yyyy") + " - " + end.ToString("dd MMMM yyyy");
                IsMultiselect = true;
                DataList.Clear();
                MultiselectDatas(start, end);
            }
        }

        private ObservableCollection<object> MultiselectDatas(DateTime start, DateTime end)
        {
            for (DateTime date = start; date <= end; date = date.AddDays(1.0))
            {
                if (GetEvents.Any())
                {
                    foreach (Event e in GetEvents)
                    {
                        DateTime startDate = e.StartDate.Date.AddDays(-1);
                        DateTime finalDate = e.EndDate.Date.AddDays(1);
                        if (date < finalDate && date > startDate && !DataList.Contains(e))
                        {
                            DataList.Add(e);
                        }
                    }
                }

                if (GetReminders.Any())
                {
                    foreach (Reminder r in GetReminders)
                    {
                        if (r.ReminderDate.Date == date && !DataList.Contains(r))
                        {
                            DataList.Add(r);
                        }
                    }
                }
            }
            return DataList;
        }

        [Obsolete]
        private async Task TappedEvent(object param)
        {
            ItemTappedEventArgs args = param as ItemTappedEventArgs;
            object item = args.Item;

            if (item is Event)
            {
                await _pageService.PushAsync(new EventDetailPage(item)).ConfigureAwait(false);
            }
            else if (item is Reminder)
            {
                await _pageService.PushAsync(new ReminderDetailPage(item)).ConfigureAwait(false);
            }
        }

        private void ClickFab()
        {
            if (EventVisiblity == false && ReminderVisiblity == false)
            {
                EventVisiblity = true;
                ReminderVisiblity = true;
            }
            else
            {
                EventVisiblity = false;
                ReminderVisiblity = false;
            }
        }

        [Obsolete]
        private async Task CreateEvent()
        {
            await _pageService.PushAsync(new CreateEventPage("create", null)).ConfigureAwait(false);

            EventVisiblity = false;
            ReminderVisiblity = false;
        }

        private async Task CreateReminder()
        {
            await _pageService.PushAsync(new CreateReminderPage("create", null)).ConfigureAwait(false);

            EventVisiblity = false;
            ReminderVisiblity = false;
        }

        private async void OnList()
        {
            try
            {
                await _pageService.PushAsync(new EventListPage(SelectedDateText, IsMultiselect)).ConfigureAwait(false);
                _isRefresh = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return;
            }
        }

        private void RefreshCalendar()
        {
            _isRefresh = true;
            IsRefreshing = true;
            RefreshAttendances();
            IsRefreshing = false;
        }

        private bool _eventVisiblity;
        public bool EventVisiblity
        {
            get => _eventVisiblity;
            set => SetValue(ref _eventVisiblity, value);
        }

        private bool _reminderVisiblity;
        public bool ReminderVisiblity
        {
            get => _reminderVisiblity;
            set => SetValue(ref _reminderVisiblity, value);
        }

        private bool _isMultiselect = false;
        public bool IsMultiselect
        {
            get => _isMultiselect;
            set => SetValue(ref _isMultiselect, value);
        }

        private string _selectedDateText;
        public string SelectedDateText
        {
            get => _selectedDateText;
            set => SetValue(ref _selectedDateText, value);
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetValue(ref _selectedDate, value);
        }

        private ObservableCollection<SpecialDate> attendances;
        public ObservableCollection<SpecialDate> Attendances
        {
            get => attendances;
            private set => SetValue(ref attendances, value);
        }

        private List<SpecialDate> _dates;
        public List<SpecialDate> dates
        {
            get => _dates;
            private set => SetValue(ref _dates, value);
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetValue(ref isRefreshing, value);
        }
    }
}

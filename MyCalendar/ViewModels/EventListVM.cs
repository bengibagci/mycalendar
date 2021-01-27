using MyCalendar.Models;
using MyCalendar.Services;
using MyCalendar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class EventListVM : BaseViewModel
    {
        public static bool IsUpdatedEvent = true;
        public static bool IsUpdatedReminder = true;
        public static bool IsDeleted = true;

        private readonly IPageService _pageService;
        private readonly IEventService _eventService;
        private readonly IReminderService _reminderService;

        private bool _isDataLoaded;
        private bool isMultiselect;

        private List<Reminder> GetReminders { get; set; }
        private List<Event> GetEvents { get; set; }
        private List<DateTime> TabDates { get; set; } = new List<DateTime>();

        private ObservableCollection<object> List { get; set; } = new ObservableCollection<object>();

        public ObservableCollection<object> CList { get; private set; } = new ObservableCollection<object>();

        public ICommand LoadDataCommand { get; private set; }
        public ICommand TappedEventCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public EventListVM(IPageService pageService, string SelectedDateText, bool IsMultiselect)
        {
            _pageService = pageService;
            _eventService = new EventService(DependencyService.Get<ISQLiteDb>());
            _reminderService = new ReminderService(DependencyService.Get<ISQLiteDb>());

            PageTitle = SelectedDateText;
            isMultiselect = IsMultiselect;
            TabDates = CalendarPageVM.TabDates;

            //LoadData();

            LoadDataCommand = new Command(() => LoadData());
            TappedEventCommand = new Command(async (param) => await TappedEvent(param).ConfigureAwait(false));
            SearchCommand = new Command((param) => Search(param));
        }

        private async void LoadData()
        {
            if (_isDataLoaded)
            {
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

            if (isMultiselect == false)
            {
                DateTime selectedDate = TabDates.OrderBy(x => x).First();
                SelectedData(selectedDate);
            }
            else
            {
                DateTime start = TabDates.OrderBy(x => x).First();
                DateTime end = TabDates.OrderBy(x => x).Last();
                MultiselectDatas(start, end);
            }
        }

        private ObservableCollection<object> SelectedData(DateTime date)
        {
            if (GetEvents.Any())
            {
                foreach (Event e in GetEvents)
                {
                    DateTime startDate = e.StartDate.Date.AddDays(-1);
                    DateTime finalDate = e.EndDate.Date.AddDays(1);
                    if (date < finalDate && date > startDate)
                    {
                        CList.Add(e);
                        List.Add(e);
                    }
                }
            }

            if (GetReminders.Any())
            {
                foreach (Reminder r in GetReminders)
                {
                    if (r.ReminderDate.Date == date)
                    {
                        CList.Add(r);
                        List.Add(r);
                    }
                }
            }

            Thread.Sleep(1000);
            return CList;
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
                        if (date < finalDate && date > startDate && !CList.Contains(e))
                        {
                            CList.Add(e);
                            List.Add(e);
                        }
                    }
                }

                if (GetReminders.Any())
                {
                    foreach (Reminder r in GetReminders)
                    {
                        if (r.ReminderDate.Date == date && !CList.Contains(r))
                        {
                            CList.Add(r);
                            List.Add(r);
                        }
                    }
                }
            }
            return CList;
        }

        private async Task<ObservableCollection<object>> UpdateEvent()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);

            if (GetEvents.Any())
            {
                Event updatedEvent = GetEvents.Where(x => x.UpdateDate != null).OrderBy(x => x.UpdateDate).Last();

                foreach (object i in CList)
                {
                    if (i is Event item)
                    {
                        if (item.Id == updatedEvent.Id)
                        {
                            CList.Remove(item);
                            CList.Add(updatedEvent);

                            List.Remove(item);
                            List.Add(updatedEvent);

                            break;
                        }
                    }
                }
            }

            IsUpdatedEvent = true;

            Thread.Sleep(1000);
            return CList;
        }

        private async Task<ObservableCollection<object>> UpdateReminder()
        {
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            if (GetReminders.Any())
            {
                Reminder updatedReminder = GetReminders.Where(x => x.UpdateDate != null).OrderBy(x => x.UpdateDate).Last();

                foreach (object i in CList)
                {
                    if (i is Reminder item)
                    {
                        if (item.Id == updatedReminder.Id)
                        {
                            CList.Remove(item);
                            CList.Add(updatedReminder);

                            List.Remove(item);
                            List.Add(updatedReminder);

                            break;
                        }
                    }
                }
            }

            IsUpdatedReminder = true;

            Thread.Sleep(1000);
            return CList;
        }

        private async Task<ObservableCollection<object>> Delete()
        {
            GetEvents = await _eventService.GetEventsAsync().ConfigureAwait(false);
            GetReminders = await _reminderService.GetRemindersAsync().ConfigureAwait(false);

            int count = 0;
            while (count < CList.Count)
            {
                if (CList[count] is Event eventModel)
                {
                    if (!GetEvents.Any(x => x.Id == eventModel.Id))
                    {
                        CList.Remove(eventModel);
                        List.Remove(eventModel);
                    }
                }
                else if (CList[count] is Reminder reminderModel)
                {
                    if (!GetReminders.Any(x => x.Id == reminderModel.Id))
                    {
                        CList.Remove(reminderModel);
                        List.Remove(reminderModel);
                    }
                }

                count++;
            }

            IsDeleted = true;

            Thread.Sleep(1000);
            return CList;
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

        private ObservableCollection<object> Search(object param)
        {
            string key = (param as TextChangedEventArgs).NewTextValue;

            var list = List.Where(x => (x is Event == true) ? (x as Event).Title.Contains(key) : (x as Reminder).Title.Contains(key)).ToList();

            CList.Clear();
            foreach (var i in list)
            {
                CList.Add(i);
            }

            return CList;
        }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetValue(ref _pageTitle, value);
        }
    }
}

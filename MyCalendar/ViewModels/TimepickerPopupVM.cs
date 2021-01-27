using MyCalendar.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class TimepickerPopupVM : BaseViewModel
    {
        public List<Timepicker> HoldHour { get; private set; } = new List<Timepicker>();
        public List<Timepicker> HoldMinute { get; private set; } = new List<Timepicker>();
        public ICommand OnCurrentHourChangedCommand { get; private set; }
        public ICommand OnCurrentMinuteChangedCommand { get; private set; }
        public TimepickerPopupVM()
        {
            for (int i = 0; i < 24; i++)
            {
                if (i >= 0 && i < 10)
                {
                    HoldHour.Add(new Timepicker { Hour = "0" + i.ToString() });
                }
                else
                {
                    HoldHour.Add(new Timepicker { Hour = i.ToString() });
                }
            }

            for (int i = 0; i < 60; i++)
            {
                if (i >= 0 && i < 10)
                {
                    HoldMinute.Add(new Timepicker { Minute = "0" + i.ToString() });
                }
                else
                {
                    HoldMinute.Add(new Timepicker { Minute = i.ToString() });
                }
            }

            CurrentHour = Hour + ":" + Minute;

            OnCurrentHourChangedCommand = new Command((param) => OnCurrentHourChanged(param));
            OnCurrentMinuteChangedCommand = new Command((param) => OnCurrentMinuteChanged(param));
        }

        private void OnCurrentHourChanged(object param)
        {
            CurrentItemChangedEventArgs e = param as CurrentItemChangedEventArgs;
            Timepicker item = e.CurrentItem as Timepicker;
            Hour = item.Hour;

            CurrentHour = Hour + ":" + Minute;
        }

        private void OnCurrentMinuteChanged(object param)
        {
            CurrentItemChangedEventArgs e = param as CurrentItemChangedEventArgs;
            Timepicker item = e.CurrentItem as Timepicker;
            Minute = item.Minute;

            CurrentHour = Hour + ":" + Minute;
        }

        private string _currentHour;
        public string CurrentHour
        {
            get => _currentHour;
            set => SetValue(ref _currentHour, value);
        }

        private string _hour;
        public string Hour
        {
            get => _hour;
            set => SetValue(ref _hour, value);
        }

        private string _minute;
        public string Minute
        {
            get => _minute;
            set => SetValue(ref _minute, value);
        }
    }
}

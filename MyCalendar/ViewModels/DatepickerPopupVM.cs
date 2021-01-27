using MyCalendar.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class DatepickerPopupVM : BaseViewModel
    {
        public List<Datepicker> HoldDay { get; private set; } = new List<Datepicker>();
        public List<Datepicker> HoldMonth { get; private set; } = new List<Datepicker>();
        public List<Datepicker> HoldYear { get; private set; } = new List<Datepicker>();
        public ICommand OnCurrentDayChangedCommand { get; private set; }
        public ICommand OnCurrentMonthChangedCommand { get; private set; }
        public ICommand OnCurrentYearChangedCommand { get; private set; }
        public DatepickerPopupVM()
        {
            for (int m = 1; m < 13; m++)
            {
                HoldMonth.Add(new Datepicker { Month = m.ToString() });
            }

            for (int y = DateTime.Now.Year; y < (DateTime.Now.Year + 60); y++)
            {
                HoldYear.Add(new Datepicker { Year = y.ToString() });
            }

            for (int d = 1; d < 32; d++)
            {
                HoldDay.Add(new Datepicker { Day = d.ToString() });
            }

            CurrentDate = Day + "/" + Month + "/" + Year;

            OnCurrentDayChangedCommand = new Command((param) => OnCurrentDayChanged(param));
            OnCurrentMonthChangedCommand = new Command((param) => OnCurrentMonthChanged(param));
            OnCurrentYearChangedCommand = new Command((param) => OnCurrentYearChanged(param));
        }

        private void OnCurrentDayChanged(object param)
        {
            CurrentItemChangedEventArgs e = param as CurrentItemChangedEventArgs;
            Datepicker item = e.CurrentItem as Datepicker;
            Day = item.Day;

            CurrentDate = Day + "/" + Month + "/" + Year;
        }

        private void OnCurrentMonthChanged(object param)
        {
            CurrentItemChangedEventArgs e = param as CurrentItemChangedEventArgs;
            Datepicker item = e.CurrentItem as Datepicker;
            Month = item.Month;

            CurrentDate = Day + "/" + Month + "/" + Year;
        }

        private void OnCurrentYearChanged(object param)
        {
            CurrentItemChangedEventArgs e = param as CurrentItemChangedEventArgs;
            Datepicker item = e.CurrentItem as Datepicker;
            Year = item.Year;

            CurrentDate = Day + "/" + Month + "/" + Year;
        }

        private string _currentDate;
        public string CurrentDate
        {
            get => _currentDate;
            set => SetValue(ref _currentDate, value);
        }

        private string _day;
        public string Day
        {
            get => _day;
            set => SetValue(ref _day, value);
        }

        private string _month;
        public string Month
        {
            get => _month;
            set => SetValue(ref _month, value);
        }

        private string _year;
        public string Year
        {
            get => _year;
            set => SetValue(ref _year, value);
        }
    }
}

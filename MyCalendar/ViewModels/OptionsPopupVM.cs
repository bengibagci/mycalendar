using MyCalendar.Enums;
using MyCalendar.Models;
using MyCalendar.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class OptionsPopupVM : BaseViewModel
    {
        private readonly IPageService _pageService;
        public List<PopupItem> PopupItems { get; private set; } = new List<PopupItem>();

        public List<PopupItem> NoticePopupItems()
        {
            PopupItems.Add(new PopupItem { Id = Notice.None, Title = "None" });
            PopupItems.Add(new PopupItem { Id = Notice.EventTime, Title = "At event time" });
            PopupItems.Add(new PopupItem { Id = Notice.FiveMinutes, Title = "5 minutes" });
            PopupItems.Add(new PopupItem { Id = Notice.TenMinutes, Title = "10 minutes" });
            PopupItems.Add(new PopupItem { Id = Notice.FifteenMinutes, Title = "15 minutes" });
            PopupItems.Add(new PopupItem { Id = Notice.ThirtyMinutes, Title = "30 minutes" });
            PopupItems.Add(new PopupItem { Id = Notice.OneHour, Title = "1 hour" });
            PopupItems.Add(new PopupItem { Id = Notice.TwoHours, Title = "2 hours" });
            PopupItems.Add(new PopupItem { Id = Notice.OneDay, Title = "1 day" });
            PopupItems.Add(new PopupItem { Id = Notice.TwoDays, Title = "2 days" });

            return PopupItems;
        }

        public ICommand OnTappedOptionCommand { get; private set; }

        [Obsolete]
        public OptionsPopupVM()
        {
            NoticePopupItems();

            OnTappedOptionCommand = new Command<PopupItem>(param => OnTappedOption(param));
        }

        [Obsolete]
        private void OnTappedOption(object param)
        {
            PopupItem item = param as PopupItem;
            MessagingCenter.Send(this, "SelectedNotice", item);

            PopupNavigation.PopAsync();
        }
    }
}

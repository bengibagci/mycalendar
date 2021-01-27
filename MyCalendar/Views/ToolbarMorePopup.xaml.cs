using MyCalendar.Models;
using MyCalendar.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolbarMorePopup : PopupPage
    {
        private readonly IPageService _pageService;
        private readonly object MyObject;
        public List<string> actions { get; private set; } = new List<string>();

        public ToolbarMorePopup(object myObject)
        {
            InitializeComponent();
            _pageService = new PageService();
            MyObject = myObject;

            actions.Add("Edit");
            actions.Add("Delete");

            popupList.FlowItemsSource = actions;
        }

        private void OnBackgroundClick(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        [Obsolete]
        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string item = e.Item.ToString();

            if (item == "Edit")
            {
                PopupNavigation.Instance.PopAsync();

                if (MyObject is Event eventModel)
                {
                    _pageService.PushAsync(new CreateEventPage("edit", eventModel));
                }
                else if (MyObject is Reminder reminderModel)
                {
                    _pageService.PushAsync(new CreateReminderPage("edit", reminderModel));
                }
            }
            else if (item == "Delete")
            {
                PopupNavigation.Instance.PopAsync();
                PopupNavigation.PushAsync(new DeletePopup(MyObject));
            }
        }
    }
}
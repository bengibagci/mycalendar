using MyCalendar.Models;
using MyCalendar.Services;
using MyCalendar.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePopup : PopupPage
    {
        private readonly IPageService _pageService;
        private readonly IEventService eventService;
        private readonly IReminderService reminderService;
        private readonly object MyObject;

        private DeletePopupsVM vm { get; set; }
        public DeletePopup(object myObject)
        {
            InitializeComponent();
            _pageService = new PageService();
            eventService = new EventService(DependencyService.Get<ISQLiteDb>());
            reminderService = new ReminderService(DependencyService.Get<ISQLiteDb>());

            vm = new DeletePopupsVM(myObject);
            BindingContext = vm;

            MyObject = myObject;
        }

        private void OnBackgroundClick(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnOk(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();

            if (MyObject is Event eventModel)
            {
                eventService.DeleteEvent(eventModel);
            }
            else if (MyObject is Reminder reminderModel)
            {
                reminderService.DeleteReminder(reminderModel);
            }

            CalendarPageVM.IsDeleted = false;
            EventListVM.IsDeleted = false;
            Thread.Sleep(500);
            _pageService.PopAsync();
        }
    }
}
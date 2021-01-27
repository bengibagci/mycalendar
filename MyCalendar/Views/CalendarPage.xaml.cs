using MyCalendar.Services;
using MyCalendar.ViewModels;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly IPageService pageService;
        private readonly IEventService eventService;
        private readonly IReminderService reminderService;

        private CalendarPageVM viewModel { get; set; }

        [Obsolete]
        public CalendarPage()
        {
            InitializeComponent();

            pageService = new PageService();
            eventService = new EventService(DependencyService.Get<ISQLiteDb>());
            reminderService = new ReminderService(DependencyService.Get<ISQLiteDb>());

            viewModel = new CalendarPageVM(pageService, eventService, reminderService);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            viewModel.LoadDataCommand.Execute(null);

            base.OnAppearing();
            Thread.Sleep(1000);
            viewModel.RefreshAttendancesCommand.Execute(null);
        }

        private void TappedEvent(object sender, ItemTappedEventArgs e)
        {
            viewModel.TappedEventCommand.Execute(e);
        }
    }
}
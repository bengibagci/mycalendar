using MyCalendar.Services;
using MyCalendar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventListPage : ContentPage
    {
        EventListVM vm;

        public EventListPage(string SelectedDateText, bool IsMultiselect)
        {
            InitializeComponent();

            vm = new EventListVM(new PageService(), SelectedDateText, IsMultiselect);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.LoadDataCommand.Execute(null);

            base.OnAppearing();
        }

        private void TappedEvent(object sender, ItemTappedEventArgs e)
        {
            vm.TappedEventCommand.Execute(e);
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            flowList.BeginRefresh();
            vm.SearchCommand.Execute(e);
            flowList.EndRefresh();
        }
    }
}
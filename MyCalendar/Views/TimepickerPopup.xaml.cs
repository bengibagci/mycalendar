using MyCalendar.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimepickerPopup : PopupPage
    {
        private readonly string AutomationId;

        private TimepickerPopupVM vm { get; set; }
        public TimepickerPopup(string automationId)
        {
            InitializeComponent();

            vm = new TimepickerPopupVM();
            BindingContext = vm;

            AutomationId = automationId;
        }

        private void OnBackgroundClick(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnCurrentHourChanged(object sender, CurrentItemChangedEventArgs e)
        {
            vm.OnCurrentHourChangedCommand.Execute(e);
        }

        private void OnCurrentMinuteChanged(object sender, CurrentItemChangedEventArgs e)
        {
            vm.OnCurrentMinuteChangedCommand.Execute(e);
        }

        private void OnOk(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "SelectedTime", (AutomationId, vm.CurrentHour));

            PopupNavigation.Instance.PopAsync();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
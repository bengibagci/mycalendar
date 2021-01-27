using MyCalendar.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatepickerPopup : PopupPage
    {
        private readonly string AutomationId;

        private DatepickerPopupVM vm { get; set; }
        public DatepickerPopup(string automationId)
        {
            InitializeComponent();
            vm = new DatepickerPopupVM();
            BindingContext = vm;

            AutomationId = automationId;
        }

        private void OnBackgroundClick(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnCurrentDayChanged(object sender, CurrentItemChangedEventArgs e)
        {
            vm.OnCurrentDayChangedCommand.Execute(e);
        }

        private void OnCurrentMonthChanged(object sender, CurrentItemChangedEventArgs e)
        {
            vm.OnCurrentMonthChangedCommand.Execute(e);
        }

        private void OnCurrentYearChanged(object sender, CurrentItemChangedEventArgs e)
        {
            vm.OnCurrentYearChangedCommand.Execute(e);
        }

        private void OnOk(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "SelectedDay", (AutomationId, vm.CurrentDate));

            PopupNavigation.Instance.PopAsync();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
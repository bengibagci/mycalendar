using MyCalendar.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class DetailVM : BaseViewModel
    {
        public ICommand MoreCommand { get; private set; }
        public object Selected { get; private set; }

        [Obsolete]
        public DetailVM(object myObject)
        {
            Selected = myObject;

            MoreCommand = new Command(() => More(myObject));
        }

        [Obsolete]
        private void More(object param)
        {
            PopupNavigation.PushAsync(new ToolbarMorePopup(param));
        }
    }
}

using MyCalendar.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPopup : PopupPage
    {
        private readonly OptionsPopupVM viewModel;

        [Obsolete]
        public OptionsPopup()
        {
            InitializeComponent();

            viewModel = new OptionsPopupVM();
            BindingContext = viewModel;

            NoticePopup();
        }

        private void NoticePopup()
        {
            AbsoluteLayout.SetLayoutBounds(stackLayout, new Rectangle(0.5, 0.5, 0.75, 0.95));
            AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.All);

            innerStackLayout.Children.Add(new Label
            {
                Text = "Select notice",
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontFamily = Application.Current.Resources["AvenirMedium"].ToString(),
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
        }

        private void OnBackgroundClick(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
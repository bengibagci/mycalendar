using MyCalendar.Custom;
using MyCalendar.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchbar), typeof(CustomSearchbarRenderer))]
namespace MyCalendar.iOS.Custom
{
    public class CustomSearchbarRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            Control.Layer.BackgroundColor = Color.FromHex(Resx.ColorResource.PageBgColor).ToCGColor();
            Control.TintColor = Color.FromHex(Resx.ColorResource.FOBlue).ToUIColor();
            Control.Placeholder = "Search";
            Control.TextAlignment = UITextAlignment.Center;
        }
    }
}
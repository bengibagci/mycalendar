using MyCalendar.Custom;
using MyCalendar.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MyCalendar.iOS.Custom
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    UITextField textField = new UITextField();
                    SetNativeControl(textField);
                }

                Control.Layer.BorderWidth = 3;
                Control.Layer.BorderColor = Color.FromHex(Resx.ColorResource.BorderColor).ToCGColor();
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
                Control.BackgroundColor = (Color.WhiteSmoke).ToUIColor();
                Control.TintColor = Color.FromHex(Resx.ColorResource.FOBlue).ToUIColor();
            }
        }
    }
}
using MyCalendar.Custom;
using MyCalendar.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MyCalendar.iOS.Custom
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            UISwitch sw = new UISwitch
            {
                TintColor = Color.FromHex(Resx.ColorResource.SwitchOffColor).ToUIColor(),
                OnTintColor = Color.FromHex(Resx.ColorResource.FOBlue).ToUIColor()
            };

            SetNativeControl(sw);
        }
    }
}
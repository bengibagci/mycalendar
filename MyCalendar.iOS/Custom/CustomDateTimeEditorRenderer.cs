using MyCalendar.Custom;
using MyCalendar.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDateTimeEditor), typeof(CustomDateTimeEditorRenderer))]
namespace MyCalendar.iOS.Custom
{
    public class CustomDateTimeEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    UITextView textField = new UITextView();
                    SetNativeControl(textField);
                }

                Control.TextAlignment = UITextAlignment.Center;
                Control.Layer.BorderWidth = 0;
                Control.BackgroundColor = Color.White.ToUIColor();
                Control.TintColor = Color.FromHex(Resx.ColorResource.FOBlue).ToUIColor();
            }
        }
    }
}
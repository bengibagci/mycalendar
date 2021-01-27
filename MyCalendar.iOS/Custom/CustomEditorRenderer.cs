using MyCalendar.Custom;
using MyCalendar.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MyCalendar.iOS.Custom
{
    public class CustomEditorRenderer : EditorRenderer
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

                Control.Layer.BorderWidth = 3;
                Control.Layer.BorderColor = Color.FromHex(Resx.ColorResource.BorderColor).ToCGColor();
                Control.TintColor = Color.FromHex(Resx.ColorResource.FOBlue).ToUIColor();
            }
        }
    }
}
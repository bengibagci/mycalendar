using Android.Content;
using Android.Graphics;
using Android.Widget;
using MyCalendar.Custom;
using MyCalendar.Droid.Custom;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace MyCalendar.Droid.Custom
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        public CustomSwitchRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.CheckedChange -= ControlValueChanged;
                Element.Toggled -= ElementToggled;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Element.Toggled -= ElementToggled;
                return;
            }

            if (Element == null)
            {
                return;
            }

            if (Control != null)
            {
                Control.SetTrackResource(Resource.Drawable.switch_track);
                Control.SetThumbResource(Resource.Drawable.switch_thumb);

                if (Control.Checked)
                {
                    Control.TrackDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex(Resx.ColorResource.FOBlue).ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    Control.TrackDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex(Resx.ColorResource.SwitchOffColor).ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                Control.CheckedChange += OnCheckedChange;
            }
            Control.CheckedChange += ControlValueChanged;
            Element.Toggled += ElementToggled;
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (Control.Checked)
            {
                Control.TrackDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex(Resx.ColorResource.FOBlue).ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
            else
            {
                Control.TrackDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex(Resx.ColorResource.SwitchOffColor).ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }

        private void ElementToggled(object sender, ToggledEventArgs e)
        {
            Control.Checked = Element.IsToggled;
        }

        private void ControlValueChanged(object sender, EventArgs e)
        {
            Element.IsToggled = Control.Checked;
        }
    }
}
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using MyCalendar.Custom;
using MyCalendar.Droid.Custom;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MyCalendar.Droid.Custom
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(2, Xamarin.Forms.Color.FromHex(Resx.ColorResource.BorderColor).ToAndroid());
                gd.SetCornerRadius(20);
                gd.SetColor(Xamarin.Forms.Color.WhiteSmoke.ToAndroid());

                Control.SetBackground(gd);
                Control.SetPadding(20, 10, 20, 10);

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);

            }
        }
    }
}
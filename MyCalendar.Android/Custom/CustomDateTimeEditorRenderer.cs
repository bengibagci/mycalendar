using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using MyCalendar.Custom;
using MyCalendar.Droid.Custom;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDateTimeEditor), typeof(CustomDateTimeEditorRenderer))]
namespace MyCalendar.Droid.Custom
{
    [Obsolete]
    public class CustomDateTimeEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Xamarin.Forms.Color.FromHex(Resx.ColorResource.GeneralPopupCancelBgColor).ToAndroid());

                Control.SetBackground(gd);
                Control.TextAlignment = Android.Views.TextAlignment.Center;

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);
            }
        }
    }
}
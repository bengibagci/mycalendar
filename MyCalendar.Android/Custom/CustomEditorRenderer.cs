using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using MyCalendar.Custom;
using MyCalendar.Droid.Custom;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MyCalendar.Droid.Custom
{
    [Obsolete]
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(2, Xamarin.Forms.Color.FromHex(Resx.ColorResource.BorderColor).ToAndroid());
                gd.SetColor(Xamarin.Forms.Color.WhiteSmoke.ToAndroid());

                Control.SetBackground(gd);

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);
            }
        }
    }
}
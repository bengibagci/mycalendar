using Android.Content;
using Android.Runtime;
using Android.Widget;
using MyCalendar.Custom;
using MyCalendar.Droid.Custom;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchbar), typeof(CustomSearchbarRenderer))]
namespace MyCalendar.Droid.Custom
{
    public class CustomSearchbarRenderer : EntryRenderer
    {
        public CustomSearchbarRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(Color.FromHex(Resx.ColorResource.PageBgColor).ToAndroid());
                CustomSearchbar customSearchbar = (CustomSearchbar)Element;
                customSearchbar.Placeholder = "Search";
                customSearchbar.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
                Control.Gravity = Android.Views.GravityFlags.CenterHorizontal;


                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);
            }
        }
    }
}
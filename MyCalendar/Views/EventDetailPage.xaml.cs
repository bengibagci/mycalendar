﻿using MyCalendar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailPage : ContentPage
    {
        private DetailVM vm { get; set; }

        [System.Obsolete]
        public EventDetailPage(object myObject)
        {
            InitializeComponent();

            vm = new DetailVM(myObject);
            BindingContext = vm;

            ToolbarItem More = new ToolbarItem
            {
                IconImageSource = "ToolbarMoreIcon.png",
                Command = vm.MoreCommand,
                Priority = 0
            };
            ToolbarItems.Add(More);
        }
    }
}
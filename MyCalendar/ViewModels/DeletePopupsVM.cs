using MyCalendar.Models;

namespace MyCalendar.ViewModels
{
    public class DeletePopupsVM : BaseViewModel
    {
        public DeletePopupsVM(object myObject)
        {
            Title = "Delete";

            if (myObject is Event eventModel)
            {
                Content = "Are you sure you want to delete " + eventModel.Title + "?";
            }
            else if (myObject is Reminder reminderModel)
            {
                Content = "Are you sure you want to delete " + reminderModel.Title + "?";
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}

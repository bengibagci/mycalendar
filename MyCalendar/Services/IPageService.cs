using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCalendar.Services
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task<Page> PopAsync();
        Task PopToRootAsync();
        void InsertPageBefore(Page page, Page before);
    }
}

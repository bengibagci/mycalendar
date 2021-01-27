using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCalendar.Services
{
    public class PageService : IPageService
    {
        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task<Page> PopAsync()
        {
            return await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task PopToRootAsync()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        public void InsertPageBefore(Page page, Page before)
        {
            Application.Current.MainPage.Navigation.InsertPageBefore(page, before);
        }
    }
}

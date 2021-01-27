using MyCalendar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCalendar.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event> GetEvent(int id);
        Task<int> AddEvent(Event e);
        Task<int> UpdateEvent(Event e);
        Task<int> DeleteEvent(Event e);
    }
}

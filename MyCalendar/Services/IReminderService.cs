using MyCalendar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCalendar.Services
{
    public interface IReminderService
    {
        Task<List<Reminder>> GetRemindersAsync();
        Task<Reminder> GetReminder(int id);
        Task<int> AddReminder(Reminder r);
        Task<int> UpdateReminder(Reminder r);
        Task<int> DeleteReminder(Reminder r);
    }
}

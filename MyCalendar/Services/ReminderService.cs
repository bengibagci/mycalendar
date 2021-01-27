using MyCalendar.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCalendar.Services
{
    public class ReminderService : IReminderService
    {
        private readonly SQLiteAsyncConnection _connection;
        public ReminderService(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<Reminder>();
        }
        public Task<List<Reminder>> GetRemindersAsync()
        {
            return _connection.Table<Reminder>().ToListAsync();
        }
        public async Task<int> DeleteReminder(Reminder r)
        {
            return await _connection.DeleteAsync(r).ConfigureAwait(false);
        }
        public async Task<int> AddReminder(Reminder r)
        {
            return await _connection.InsertAsync(r).ConfigureAwait(false);
        }
        public async Task<int> UpdateReminder(Reminder r)
        {
            return await _connection.UpdateAsync(r).ConfigureAwait(false);
        }
        public async Task<Reminder> GetReminder(int id)
        {
            return await _connection.FindAsync<Reminder>(id).ConfigureAwait(false);
        }
    }
}

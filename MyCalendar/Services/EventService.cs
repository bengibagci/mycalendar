using MyCalendar.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCalendar.Services
{
    public class EventService : IEventService
    {
        private readonly SQLiteAsyncConnection _connection;
        public EventService(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<Event>();
        }
        public Task<List<Event>> GetEventsAsync()
        {
            return _connection.Table<Event>().ToListAsync();
        }
        public async Task<int> DeleteEvent(Event e)
        {
            return await _connection.DeleteAsync(e).ConfigureAwait(false);
        }
        public async Task<int> AddEvent(Event e)
        {
            return await _connection.InsertAsync(e).ConfigureAwait(false);
        }
        public async Task<int> UpdateEvent(Event e)
        {
            return await _connection.UpdateAsync(e).ConfigureAwait(false);
        }
        public async Task<Event> GetEvent(int id)
        {
            return await _connection.FindAsync<Event>(id).ConfigureAwait(false);
        }
    }
}

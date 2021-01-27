using SQLite;

namespace MyCalendar
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}


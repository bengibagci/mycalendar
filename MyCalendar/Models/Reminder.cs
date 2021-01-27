using MyCalendar.Enums;
using SQLite;
using System;

namespace MyCalendar.Models
{
    [Table("Reminders")]
    public class Reminder
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime ReminderDate { get; set; }

        public Priority Priority { get; set; }

        public bool IsEvent { get; set; }

        public string IconPath { get; set; }
    }
}

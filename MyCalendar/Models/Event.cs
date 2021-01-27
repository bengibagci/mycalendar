using MyCalendar.Enums;
using SQLite;
using System;

namespace MyCalendar.Models
{
    [Table("Events")]
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(999)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Notice Notice { get; set; }

        public bool IsFullDay { get; set; }

        public bool IsOneDay { get; set; }

        public bool IsEvent { get; set; }

        public string IconPath { get; set; }
    }
}

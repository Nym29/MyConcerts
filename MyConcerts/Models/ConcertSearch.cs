using System;
using System.Collections.Generic;
using SQLite;

namespace MyConcerts.Models
{
    [Table("ConcertSearch")]
    public class ConcertSearch
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; }

        public string MusicStyle { get; set; }

        public ConcertSearch()
        {
        }
    }
}

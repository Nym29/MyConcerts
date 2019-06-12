using System;
using System.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MyConcerts.Models
{
    [Table("TrackedConcerts")]
    public class ConcertDetails
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string EventID { get; set; }

        public string AttractionID { get; set; }

        public string EventName { get; set; }

        public string Location { get; set; }

        public string Venue { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        [ForeignKey(typeof(Artist))]
        public int ArtistId { get; set; }

        [OneToOne]
        public Artist Artist { get; set; }

        public string ImageUrl { get; set; }

        public string BuyTicketsUrl { get; set; }

        public string Notes { get; set; }

        public string Setlist { get; set; }

        public ConcertDetails()
        {

        }
    }
}

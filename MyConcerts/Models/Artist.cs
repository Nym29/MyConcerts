using SQLite;

namespace MyConcerts.Models
{
    [Table("Artists")]
    public class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Mbid { get; set; }

        public string Tmid { get; set; }

        public string Name { get; set; }

        public Artist()
        {
        }
    }
}



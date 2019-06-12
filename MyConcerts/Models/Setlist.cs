using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SQLite;

namespace MyConcerts.Models
{
    [Table("Setlists")]
    public class Setlist
    {
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public JObject Sets { get; set; }
        public string Songs { get; set; }

        public Setlist()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Entities
{
    public class Movie
    {
        public string NameMovie { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Subtitle { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Status { get; set; }
    }
}
}

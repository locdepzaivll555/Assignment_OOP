using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Entities
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone{ get; set; }

        public bool Status { get; set; }/// false: user, true: admin
    }
    public class TicketMovie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MovieName { get; set; }
        public string Room { get; set; }
        public string[,] Seat { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
    }
    public class Movie
    {
        public string NameMovie { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public string Subtitle { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Status { get; set; }
    }
}

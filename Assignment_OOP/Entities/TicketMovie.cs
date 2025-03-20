
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Entities
{
    
    public class TicketMovie
    {
        public int Id { get; set; }
        public string NameCinema { get; set; }
        public string MovieName { get; set; }
        public string Room { get; set; }
        public string[,] Seat { get; set; }
        public DateTime ShowTime { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
    }
    



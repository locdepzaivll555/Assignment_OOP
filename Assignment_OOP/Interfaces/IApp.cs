using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Interfaces
{
    internal interface ITicketApp<V>
    {
        
        public void MainMenu();
        public void ShowMoviw();
        public void ShowMovies();
        public void ShowTime();
        public void ShowSeat();
        public void ShowPrice();
        public void ShowTicket();
    }
}

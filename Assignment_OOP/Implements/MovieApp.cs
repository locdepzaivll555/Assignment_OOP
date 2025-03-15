using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Implements
{
    public abstract class MovieApp <Model>
    {
        public abstract void MainMenu();
        public abstract Model UpdateMovie();
        public abstract void ShowMovies();
        public abstract void ChoseMovie();
        public abstract void ShowTime();
        public abstract void ChoseTime();
        public abstract void ShowSeat();
        public abstract void ChoseSeat();
        public abstract void ShowPrice();
        public abstract void Payment();
        public abstract void ShowTicket();

    }
}

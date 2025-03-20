using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_OOP.Implements
{
    public abstract class SApp <Model>
    {
         
        public abstract int ChoseMovie();
        public abstract int ChoseTime();  
        public abstract int ChoseSeat();
        public abstract int Payment();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public abstract Model UpdateMovie();/// Hoàn thành sau 
    }
}

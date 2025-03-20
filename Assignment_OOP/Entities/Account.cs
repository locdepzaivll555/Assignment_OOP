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

        public string Phone { get; set; }

        public bool Status { get; set; }/// false: user, true: admin
    }
}

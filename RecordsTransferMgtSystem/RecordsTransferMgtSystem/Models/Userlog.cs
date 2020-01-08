using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordsTransferMgtSystem.Models
{
    public class Userlog
    {
        public int id { get; set; } 
       public string Firstname{ get; set; }
       public string Lastname { get; set; }  
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string role { get; set; }
        public string password { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShaulisBlog.Models
{
    // Class which represents a fan
    public class Fan
    {
        public int FanID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public DateTime BDate { get; set; }
        public int Seniority { get; set; }
    }

}
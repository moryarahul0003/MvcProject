using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationDetails.Models
{
    public class UserList
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CustomerAddress { get; set; }
        public string City { get; set; }
        public string images { get; set; }
    }
}
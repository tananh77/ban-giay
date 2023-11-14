using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class AccountModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public bool? Sex { get; set; }
        public string Image { get; set; }
    }

}
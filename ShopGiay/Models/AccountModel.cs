using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class AccountModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string Role { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone must contain only numbers.")]
        [StringLength(10, ErrorMessage = "Phone must be exactly 10 digits.", MinimumLength = 10)]
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool? Sex { get; set; }
        public string Image { get; set; }
        public string OldPassword { get; set; }
    }

}
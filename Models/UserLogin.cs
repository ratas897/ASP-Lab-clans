using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP_Lab_clans.Models
{
    public class UserLogin
    {
        [Display(Name ="Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Remember login")]
        public bool Remember { get; set; }
    }
}
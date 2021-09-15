using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP_Lab_clans.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name ="Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Username is required")]
        public string Username { get; set; }
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> Dateofbirth { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [MinLength(6,ErrorMessage ="Password must be atleast 6 characters")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
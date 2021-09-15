using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP_Lab_clans.Models
{
    public class ClanClass
    {
        [Display(Name = "Clan name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Clan name is required")]
        public string Clan_name { get; set; }

        [Display(Name = "Clan tag")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Clan tag is required")]
        [MaxLength(4, ErrorMessage = "Tag must at most 4 characters")]
        public string Clan_tag { get; set; }


        [Display(Name = "Clan message")]
        public string Clan_message { get; set; }

    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP_Lab_clans.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int id { get; set; }
        public Nullable<int> Clan_ID { get; set; }
        public Nullable<int> sender { get; set; }
        public string message1 { get; set; }
    
        public virtual Clan Clan { get; set; }
        public virtual User User { get; set; }
    }
}

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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Clan_member> Clan_member { get; set; }
        public virtual DbSet<Clan_roles> Clan_roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Clan> Clans { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}

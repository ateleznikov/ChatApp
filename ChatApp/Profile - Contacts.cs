using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace ChatApp
{
    public class Profile
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BIO { get; set; }
        public string ProfilePicture { get; set; }
        public List<Contacts> Contacts { get; set; } 

    }
    public class Contacts
    {
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public Profile Profile { get; set; } 
        public virtual Profile RelatedContact { get; set; }
    }
    
    public class ContactsData : DbContext
    {
        public ContactsData() : base("ChatApp")
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
    }
}

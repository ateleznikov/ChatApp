using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace ChatApp
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Profile> Profiles { get; set; }

        public User()
        {
            Profiles = new List<Profile>();
        }
    }

    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BIO { get; set; }
        public string ProfilePicture { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Contacts> Contacts { get; set; }

        public Profile()
        {
            Contacts = new List<Contacts>();
        }
    }

    public class ChatMessageV2
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual Contacts Contact { get; set; }
    }

    public class Contacts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        public int RelatedContactId { get; set; }
        public virtual Profile RelatedContact { get; set; }
        public virtual ICollection<ChatMessageV2> ChatMessages { get; set; }

        public Contacts()
        {
            ChatMessages = new List<ChatMessageV2>();
        }
    }

    public class ContactsData : DbContext
    {
        public ContactsData() : base("ChatApp")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<ChatMessageV2> ChatMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasRequired(p => p.User)
                .WithMany(u => u.Profiles)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Contacts>()
                .HasRequired(c => c.RelatedContact)
                .WithMany()
                .HasForeignKey(c => c.RelatedContactId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Contacts>()
                .HasRequired(c => c.RelatedContact)
                .WithMany()
                .HasForeignKey(c => c.RelatedContactId);

            modelBuilder.Entity<ChatMessageV2>()
                .HasRequired(cm => cm.Contact)
                .WithMany(c => c.ChatMessages)
                .HasForeignKey(cm => cm.ContactId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using ChatApp.Frames;
using System.Windows;

namespace ChatApp
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }

        public Profile()
        {
            Contacts = new List<Contact>();
        }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public Contact()
        {
            ChatMessages = new List<ChatMessage>();
        }
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public virtual Contact Contact { get; set; }
    }

    public class ChatAppContext : DbContext
    {
        public ChatAppContext() : base("ChatApp")
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProfileConfiguration());
            modelBuilder.Configurations.Add(new ContactConfiguration());
            modelBuilder.Configurations.Add(new ChatMessageConfiguration());
        }
    }

    public class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
            HasKey(p => p.Id);
            HasMany(p => p.Contacts)
                .WithRequired(c => c.Profile)
                .HasForeignKey(c => c.ProfileId);
        }
    }

    public class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            HasKey(c => c.Id);
            HasRequired(c => c.Profile)
                .WithMany(p => p.Contacts)
                .HasForeignKey(c => c.ProfileId);
        }
    }

    public class ChatMessageConfiguration : EntityTypeConfiguration<ChatMessage>
    {
        public ChatMessageConfiguration()
        {
            HasKey(cm => cm.Id);
            HasRequired(cm => cm.Contact)
                .WithMany(c => c.ChatMessages)
                .HasForeignKey(cm => cm.ContactId);
        }
    }

    public static class ChatAppManager
    {
        public static void AddContactFromDB(Profile profile, Contact contact)
        {
            using (var context = new ChatAppContext())
            {
                profile.Contacts.Add(contact);
                contact.Profile = profile;
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
        }
        public static void AddContact(Profile profile, Contact contact)
        {
            using (var context = new ChatAppContext())
            {

                var newProfile = new Profile
                {
                    Name = profile.Name,
                    Surname = profile.Surname,
                    Bio = profile.Bio,
                    ProfilePicture = profile.ProfilePicture
                };
                newProfile.Contacts.Add(contact);
                contact.Profile = newProfile;


                context.Profiles.Add(newProfile);
                context.Contacts.Add(contact);
                context.SaveChanges();
                (Application.Current.MainWindow as MainFrame)?.RefreshContactsList();

            }
        }

    }
}
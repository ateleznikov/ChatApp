using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ChatApp;

namespace DBCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ContactsData())
            {
                // Create a new user
                var user = new User
                {
                    Username = "john_doe",
                    Password = "password"
                };

                // Create a new profile
                var profile = new Profile
                {
                    Name = "John",
                    Surname = "Doe",
                    BIO = "Hello, I'm John Doe!",
                    ProfilePicture = "profile_picture.jpg",
                    User = user
                };

                // Create three contacts
                var contact1 = new Contacts
                {
                    Name = "Alice",
                    ProfilePicture = "alice_picture.jpg",
                    Profile = profile
                };

                var contact2 = new Contacts
                {
                    Name = "Bob",
                    ProfilePicture = "bob_picture.jpg",
                    Profile = profile
                };

                var contact3 = new Contacts
                {
                    Name = "Eve",
                    ProfilePicture = "eve_picture.jpg",
                    Profile = profile
                };

                // Add the contacts to the profile
                profile.Contacts.Add(contact1);
                profile.Contacts.Add(contact2);
                profile.Contacts.Add(contact3);

                // Add the user, profile, and contacts to the database
                db.Users.Add(user);
                db.Profiles.Add(profile);
                db.Contacts.Add(contact1);
                db.Contacts.Add(contact2);
                db.Contacts.Add(contact3);

                // Save changes to the database
                db.SaveChanges();

                Console.WriteLine("Database created successfully!");
            }
        }
    }
}

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
            // Create the database if it doesn't exist
           using (var db = new ChatAppContext())
            {
                var profile = new Profile
                {
                    Name = "John",
                    Surname = "Doe",
                    Bio = "Hello, I'm John Doe!",
                    ProfilePicture = "profile_picture.jpg"
                };

                // Create new contacts and add them to the profile
                var contact1 = new Contact
                {
                    Name = "Alice",
                    ProfilePicture = "alice_picture.jpg"
                };
                var contact2 = new Contact
                {
                    Name = "Bob",
                    ProfilePicture = "bob_picture.jpg"
                };
                var contact3 = new Contact
                {
                    Name = "Eve",
                    ProfilePicture = "eve_picture.jpg"
                };

                ChatAppManager.AddContactFromDB(profile, contact1);
                ChatAppManager.AddContactFromDB(profile, contact2);
                ChatAppManager.AddContactFromDB(profile, contact3);

                Console.WriteLine("Database created successfully!");
            }


           
        }
    }
}

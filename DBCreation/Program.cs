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
                    Name = "Anton",
                    Surname = "Teleznikov",
                    Bio = "Hello, I'm Anton Teleznikov!",
                    ProfilePicture = "../images/profile-picture.jpg"
                };

                // Create new contacts and add them to the profile
                var contact1 = new Contact
                {
                    Name = "Alice",
                    ProfilePicture = "../images/alice.jpg"
                };
                var contact2 = new Contact
                {
                    Name = "Bob",
                    ProfilePicture = "../images/bob.jpg"
                };
                var contact3 = new Contact
                {
                    Name = "Eve",
                    ProfilePicture = "../images/eve.jpg"
                };

                ChatAppManager.AddContactFromDB(profile, contact1);
                ChatAppManager.AddContactFromDB(profile, contact2);
                ChatAppManager.AddContactFromDB(profile, contact3);

                Console.WriteLine("Database created successfully!");
            }


           
        }
    }
}

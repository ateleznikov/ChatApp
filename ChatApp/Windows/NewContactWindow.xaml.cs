﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ChatApp.Frames;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace ChatApp.Frames
{
    public partial class NewContactWindow : Window
    {
        private ChatAppContext db;
        public NewContactWindow()
        {
            InitializeComponent();
            db = new ChatAppContext();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            var profile = db.Profiles.FirstOrDefault();

            string name = NameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a contact name.");
                return;
            }

            var newContact = new Contact
            {
                Name = name,
                Profile = profile
            };

            ChatAppManager.AddContact(profile, newContact);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
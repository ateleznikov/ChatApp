﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.Entity;

namespace ChatApp.Frames
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>
    //public partial class MainFrame : Page
    //{
    //    public MainFrame()
    //    {
    //        InitializeComponent();
    //        LoadChatHistory();

    //    }
    //    private const string ChatFilePath = "C:\\ATU\\VS files\\ChatApp\\ChatApp\\history.txt";

    //    public void LoadChatHistory()
    //    {
    //        if (File.Exists(ChatFilePath))
    //        {
    //            string[] lines = File.ReadAllLines(ChatFilePath);
    //            foreach (string line in lines)
    //            {
    //                string[] parts = line.Split(new[] { "] " }, 2, StringSplitOptions.None);
    //                if (parts.Length == 2)
    //                {
    //                    string timestampString = parts[0].TrimStart('[');
    //                    string message = parts[1];

    //                    if (DateTime.TryParseExact(timestampString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
    //                    {
    //                        ChatMessage chatMessage = new ChatMessage
    //                        {
    //                            Timestamp = timestamp,
    //                            Message = message
    //                        };

    //                        ChatListBox.Items.Add(chatMessage);
    //                    }
    //                }
    //            }
    //        }
    //        if (ChatListBox.Items.Count > 0)
    //        {
    //            object lastItem = ChatListBox.Items[ChatListBox.Items.Count - 1];
    //            ChatListBox.ScrollIntoView(lastItem);
    //        }
    //    }

    //    private void SendButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        SendMessage();
    //    }

    //    private void ChatInputTextBox_KeyDown(object sender, KeyEventArgs e)
    //    {
    //        if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Shift)
    //        {
    //            ChatInputTextBox.Text += Environment.NewLine;
    //            ChatInputTextBox.CaretIndex = ChatInputTextBox.Text.Length;
    //            e.Handled = true;
    //        }
    //        else if (e.Key == Key.Enter)
    //        {
    //            SendMessage();
    //        }
    //    }

    //    public void SendMessage()
    //    {


    //        string message = ChatInputTextBox.Text.Trim();
    //        DateTime now = DateTime.Now;
    //        ChatMessage chatMessage = new ChatMessage
    //        {
    //            Timestamp = now,
    //            Message = message
    //        };
    //        if (string.IsNullOrWhiteSpace(message))
    //        {
    //            // Does not allow to send empty messages and spaces
    //            return;
    //        }
    //        File.AppendAllText(ChatFilePath, chatMessage.ToString() + Environment.NewLine);
    //        ChatListBox.Items.Add(chatMessage);
    //        ChatInputTextBox.Clear();
    //        if (ChatListBox.Items.Count > 0)
    //        {
    //            // This if scrolls to the bottom of the Lisbox when the new message is added
    //            object lastItem = ChatListBox.Items[ChatListBox.Items.Count - 1];
    //            ChatListBox.ScrollIntoView(lastItem);
    //        }
    //    }


    //}

    public partial class MainFrame : Page
    {
        private ContactsData _db;

        public MainFrame()
        {
            InitializeComponent();
            _db = new ContactsData();
            LoadChatHistory();
            LoadContacts();
        }
        public void LoadContacts()
        {
            ContactsComboBox.ItemsSource = _db.Contacts.ToList();
        }
        public void LoadChatHistory()
        {
            var selectedContact = (Contacts)ContactsComboBox.SelectedItem;
            if (selectedContact == null)
            {
                // No contact selected, return or show an error message
                return;
            }

            var chatMessages = _db.ChatMessages
                .Include(cm => cm.Contact)
                .Where(cm => cm.ContactId == selectedContact.Id)
                .ToList();

            ChatListBox.Items.Clear(); // Clear existing items

            foreach (var chatMessage in chatMessages)
            {
                ChatListBox.Items.Add(chatMessage);
            }

            if (ChatListBox.Items.Count > 0)
            {
                object lastItem = ChatListBox.Items[ChatListBox.Items.Count - 1];
                ChatListBox.ScrollIntoView(lastItem);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void ChatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Shift)
            {
                ChatInputTextBox.Text += Environment.NewLine;
                ChatInputTextBox.CaretIndex = ChatInputTextBox.Text.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        public void SendMessage()
        {
            string message = ChatInputTextBox.Text.Trim();
            DateTime now = DateTime.Now;

            var selectedContact = (Contacts)ContactsComboBox.SelectedItem;
            if (selectedContact == null)
            {
                MessageBox.Show("Please select a contact.");
                return;
            }

            var chatMessage = new ChatMessageV2
            {
                ContactId = selectedContact.Id,
                Message = message,
                Timestamp = now
            };

            if (string.IsNullOrWhiteSpace(message))
            {
                // Does not allow to send empty messages and spaces
                return;
            }

            _db.ChatMessages.Add(chatMessage);
            _db.SaveChanges();

            ChatListBox.Items.Add(chatMessage);
            ChatInputTextBox.Clear();

            if (ChatListBox.Items.Count > 0)
            {
                object lastItem = ChatListBox.Items[ChatListBox.Items.Count - 1];
                ChatListBox.ScrollIntoView(lastItem);
            }
        }
    }
}

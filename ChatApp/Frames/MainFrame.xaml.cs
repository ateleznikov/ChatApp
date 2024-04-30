using System;
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
using System.Runtime.Remoting.Contexts;

namespace ChatApp.Frames
{
    public partial class MainFrame : Page
    {
        private ChatAppContext db;

        public MainFrame()
        {
            InitializeComponent();
            db = new ChatAppContext();
            LoadContacts();
            LoadChatHistory();
        }

        private void ContactsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChatHistory();
        }

        private void LoadContacts()
        {
            var contacts = db.Contacts.ToList();
            ContactsListBox.ItemsSource = contacts;
        }

        private void LoadChatHistory()
        {
            Console.WriteLine("LoadChatHistory called");

            ChatListBox.Items.Clear();

            var selectedContact = (Contact)ContactsListBox.SelectedItem;
            if (selectedContact == null)
            {
                Console.WriteLine("No contact selected");
                return;
            }

            var chatMessages = db.ChatMessages
                .Where(cm => cm.ContactId == selectedContact.Id)
                .OrderBy(cm => cm.MessageTimestamp)
                .ToList();

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

        private void ChatInputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Shift)
            {
                ChatInputTextBox.Text += Environment.NewLine;
                ChatInputTextBox.CaretIndex = ChatInputTextBox.Text.Length;
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string message = ChatInputTextBox.Text.Trim();
            DateTime now = DateTime.Now;

            var selectedContact = (Contact)ContactsListBox.SelectedItem;
            if (selectedContact == null)
            {
                MessageBox.Show("Please select a contact.");
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
                return;

            var chatMessage = new ChatMessage
            {
                ContactId = selectedContact.Id,
                MessageText = message,
                MessageTimestamp = now
            };

            db.ChatMessages.Add(chatMessage);
            db.SaveChanges();

            ChatListBox.Items.Add(chatMessage);
            ChatInputTextBox.Clear();

            if (ChatListBox.Items.Count > 0)
            {
                object lastItem = ChatListBox.Items[ChatListBox.Items.Count - 1];
                ChatListBox.ScrollIntoView(lastItem);
            }
        }
        void OpenNewContactWindow(object sender, RoutedEventArgs e)
        {
            var newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();
        }
        private void RefreshContactsList()
        {
            var contacts = db.Contacts.ToList();
            ContactsListBox.ItemsSource = contacts;
        }
    }
}
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

namespace ChatApp.Frames
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>


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
        private void ContactsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChatHistory();
        }
        public void LoadContacts()
        {
            var contacts = _db.Contacts.Include(c => c.Profile).ToList();
            ContactsListBox.ItemsSource = contacts;
        }
        public void LoadChatHistory()
        {
            ChatListBox.Items.Clear(); // Clear existing items

            var selectedContact = (Contacts)ContactsListBox.SelectedItem;
            if (selectedContact == null)
            {
                // No contact selected, return or show an error message
                return;
            }

            var chatMessages = _db.ChatMessages
                .Include(cm => cm.Contact)
                .Where(cm => cm.ContactId == selectedContact.Id)
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

            var selectedContact = (Contacts)ContactsListBox.SelectedItem;
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

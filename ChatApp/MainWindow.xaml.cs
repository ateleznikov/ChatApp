using System;
using System.Collections.Generic;
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
using System.Globalization;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadChatHistory();
        }
        private const string ChatFilePath = "\\\\Mac\\Home\\Documents\\ChatApp\\ChatApp\\history.txt";
        /*private void LoadChatHistory()
        {
            if (File.Exists(ChatFilePath))
            {
                string[] messages = File.ReadAllLines(ChatFilePath);
                foreach (string message in messages)
                {
                    ChatListBox.Items.Add(message);
                }
            }
        }*/
        public void LoadChatHistory()
        {
            if (File.Exists(ChatFilePath))
            {
                string[] lines = File.ReadAllLines(ChatFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(new[] { "] " }, 2, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        string timestampString = parts[0].TrimStart('[');
                        string message = parts[1];

                        if (DateTime.TryParseExact(timestampString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
                        {
                            ChatMessage chatMessage = new ChatMessage
                            {
                                Timestamp = timestamp,
                                Message = message
                            };

                            ChatListBox.Items.Add(chatMessage);
                        }
                    }
                }
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
        /* private void SendMessage()
         {
             string message = ChatInputTextBox.Text.Trim();
             if (!string.IsNullOrEmpty(message))
             {
                  DateTime now = DateTime.Now;
                  string timestamp = now.ToString("HH:mm");
                  string fullMessage = $"[{timestamp}] {message}";
                  File.AppendAllText(ChatFilePath, fullMessage + Environment.NewLine);
                  ChatListBox.Items.Add(fullMessage);
                  ChatInputTextBox.Clear();

             }
         }*/
        public void SendMessage()
        {
            string message = ChatInputTextBox.Text.Trim();
            DateTime now = DateTime.Now;
            ChatMessage chatMessage = new ChatMessage
            {
                Timestamp = now,
                Message = message
            };

            File.AppendAllText(ChatFilePath, chatMessage.ToString() + Environment.NewLine);
            ChatListBox.Items.Add(chatMessage);
            ChatInputTextBox.Clear();
        }
    }
}

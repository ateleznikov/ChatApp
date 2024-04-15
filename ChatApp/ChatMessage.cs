using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public DateTime Timestamp {  get; set; }

        public override string ToString()
        {
            return $"[{Timestamp.ToString("HH:mm")}] {Message}";
        }
    }
}

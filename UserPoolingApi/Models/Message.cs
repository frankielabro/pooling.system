using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Position { get; set; }
    }
}

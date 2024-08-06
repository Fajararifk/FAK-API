using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int SenderID { get; set; }  // Foreign Key to Patients or Providers
        public int ReceiverID { get; set; }  // Foreign Key to Patients or Providers
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

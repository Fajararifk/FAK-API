using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public partial class MasterMessage : BaseEntity
    {
        public string Name { get; set; }

        public string MessageCode { get; set; }

        public string LanguageCode { get; set; }

        public string MessageType { get; set; }

        public string MessageText { get; set; }

        public bool? Active { get; set; }

    }
}

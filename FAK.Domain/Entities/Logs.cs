using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public partial class Logs : BaseEntity
    {
        public string Description { get; set; }

        public string Module { get; set; }

        public string ErrorLog { get; set; }

        public string Action { get; set; }

        public string Note1 { get; set; }

        public string Note2 { get; set; }


        public string Note3 { get; set; }

        public string StatusLog { get; set; }

    }
}

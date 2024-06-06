using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public partial class MasterSystemConfig : BaseEntity
    {
        public string Name { get; set; }

        public string SystemCategory { get; set; }

        public string SystemSubCategory { get; set; }

        public string SystemCode { get; set; }

        public string SystemValue { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

    }
}

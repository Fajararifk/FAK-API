using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Common.Model
{
    public class AccessMatrixModel
    {
        public string MenuCode { get; set; }
        public string Module { get; set; }
        public bool AllowInsert { get; set; }
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowExport { get; set; }
        public bool AllowImport { get; set; }
        public bool AllowApproval { get; set; }

    }
}

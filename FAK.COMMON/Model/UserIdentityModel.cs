using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Common.Model
{
    public class UserIdentityModel
    {
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public string BranchGroup { get; set; }
        public string Modules { get; set; }
        public string Host { get; set; }
    }
}

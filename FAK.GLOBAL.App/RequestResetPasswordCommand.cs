using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class RequestResetPasswordCommand
    {
        public string Email { get; set; }
        public string Browser { get; set; }
    }
}

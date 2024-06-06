using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class ResetPasswordCommand
    {
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
        public string Browser {  get; set; }
    }
}

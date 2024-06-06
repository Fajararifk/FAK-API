using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class ChangePasswordCommand
    {
        public string Password { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
        public string Browser {  get; set; }
    }
}

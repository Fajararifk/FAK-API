using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class AdditionalInformationCommand
    {
        [Key]
        public string Id {  get; set; }
        public string EmployeeId { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string BloodType { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeEmail { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string ComputerName { get; set; }
        public string StaticIPAddress { get; set; }
        public bool Glasses { get; set; }
        public string GlassesLeftRight { get; set; }
        public string Hat { get; set; }
        public string Clothers { get; set; }
        public string Jacket { get; set; }
        public string Pants { get; set; }
        public string Shoes { get; set; }
        public string Boots { get; set; }

    }
}

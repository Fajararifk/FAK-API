using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class IdentityEmployeeCommand
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string IdentityName {  get; set; }
        public string IdentityNo { get; set; }
        public DateTime IssuedDate {  get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Documents {  get; set; }
        public string Remarks { get; set; }

    }
}

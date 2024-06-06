using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class InventoryCommand
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string InventoryType { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime MustBeReturnOn { get; set;}
        public int Quantity { get; set; }
        public int Size { get; set; }
        public string Condition { get; set; }
        public string Remark { get; set; }
    }
}

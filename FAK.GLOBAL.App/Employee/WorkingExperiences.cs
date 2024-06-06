using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class WorkingExperiences
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartWorking {  get; set; }
        public DateTime EndWorking { get; set; }
        public string Company {  get; set; }
        public string BusinessField { get; set; }
        public string AddressCompany { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string ResignReason { get; set; }
        public string EmployementType { get; set; }
        public string Organization {  get; set; }
        public string JobLevel { get; set; }
        public string Description { get; set; }
        public string Reference {  get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
    }
}

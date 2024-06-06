using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class BasicDataCommand
    {
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime MarriedDate { get; set; }
        public string BPJSTK { get; set; }
        public string BPJSKES { get; set; }
        public string BPJSTKLocation { get; set; }
        public string BPJSKESLocation { get; set; }
        public string AttendaceId { get; set; }
        public string EmployementType { get; set; }
        public string Position { get; set; }
        public string Organization { get; set; }
        public string JobLevel { get; set; }
        public string JobClass {  get; set; }
        public string CostCenter { get; set; }
        public DateTime PermanentDate { get; set; }
        public DateTime PensionPlanDate { get; set; }
        public string TaxType { get; set; }
        public string TaxStatus { get; set; }
        public string NPWP { get; set; }
        public string TaxLocation { get; set; }
        public string WorkLocation { get; set; }
        public string KTP { get; set; }
        public AdditionalInformationCommand AdditionalInformation { get; set; }
    }
}

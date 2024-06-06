using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class MedicalRecordCommand
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DiseaseCategory { get; set; }
        public string DiseaseName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Therapy { get; set; }
        public string Hospital {  get; set; }
        public string Country {  get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Doctor { get; set; }
        public string Phone { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public string Remarks { get; set; }
    }
}


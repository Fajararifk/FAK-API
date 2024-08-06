using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public int PatientID { get; set; }
        public DateTime DateOfVisit { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Medications { get; set; }
        public string Allergies { get; set; }
        public string MedicalHistory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int PatientID { get; set; }  // Foreign Key to Patients
        public int AppointmentID { get; set; }  // Foreign Key to Appointments
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}

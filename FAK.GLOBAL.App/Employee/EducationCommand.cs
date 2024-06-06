using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class EducationCommand
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EducationLevel { get; set; }
        public string Faculty { get; set; }
        public string Major { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int GPA { get; set; }
        public int OfGPA { get; set; }
        public string Institution { get; set; }
        public string AddressInstitution { get; set; }
        public string CityInstitution { get; set; }
        public string GraduationType { get; set; }
        public string CertificateNo { get; set; }
        public DateTime CertificateDate { get; set; }
        public string Remark { get; set; }
        public Training Training { get; set; }
        public Skill Skill { get; set; }

    }
    public class Training
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TrainingType { get; set; }
        public string Field { get; set; }
        public string Course { get; set; }
        public string Institution { get; set; }
        public string AddressInstitution { get; set; }
        public string CityInstitution { get; set; }
        public string GraduationType { get; set; }
        public string CertificateNo { get; set; }
        public DateTime StartCertificateDate { get; set; }
        public DateTime EndCertificateDate { get; set; }
        public string PaidBy { get; set; }
        public DateTime CompanyBondUntil { get; set; }
        public string Remarks { get; set; }
    }

    public class Skill
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Description { get; set; }
        public string SkillProficiency { get; set; }
        public DateTime TakenDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Remarks { get; set; }
    }
}

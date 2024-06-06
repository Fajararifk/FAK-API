using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class EmployeeCommand
    {
        public BasicDataCommand BasicDataCommand { get; set; }
        public IdentityEmployeeCommand IdentityEmployee { get; set; }
        public AddressCommand Address { get; set; }
        public EducationCommand Education { get; set; }
        public WorkingExperiences WorkingExperiences { get; set; }
    }
}

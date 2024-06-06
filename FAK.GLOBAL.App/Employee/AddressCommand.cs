using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.Employee
{
    public class AddressCommand
    {
        [Key]
        public string Id { get; set; }
        public string AddressEmployee { get; set; }
        public string RT { get; set; }
        public string RW { get; set; }
        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string OwnerShip { get; set; }
        public string AddressEmployeeNew { get; set; }
        public string RTNew { get; set; }
        public string RWNew { get; set; }
        public string SubDistrictNew { get; set; }
        public string DistrictNew { get; set; }
        public string CityNew { get; set; }
        public string ProvinceNew { get; set; }
        public string CountryNew { get; set; }
        public string ZipCodeNew { get; set; }
        public string OwnerShipNew { get; set; }
    }
}

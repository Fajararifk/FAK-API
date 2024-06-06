using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    [NotMapped]
    public class Province
    {
        [Key]
        public int IdProvince { get; set; }
        public string nama { get; set; }
        public int id { get; set; }
        public string kode { get; set; }
        public int tingkat { get; set; }
    }
}

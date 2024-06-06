using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAK.Domain.Entities
{
    [NotMapped]
    public class HasilPemiluTPS
    {
        public int IdHasil {  get; set; }
        //public Chart chart { get; set; }
        public List<string> images { get; set; }
        public Administrasi administrasi { get; set; }
        [NotMapped]

        public object psu { get; set; }
        public string ts { get; set; }
        public bool status_suara { get; set; }
        public bool status_adm { get; set; }
    }
    [NotMapped]
    public class Administrasi
    {
        public int suara_sah { get; set; }
        public int suara_total { get; set; }
        public int pemilih_dpt_j { get; set; }
        public int pemilih_dpt_l { get; set; }
        public int pemilih_dpt_p { get; set; }
        public int pengguna_dpt_j { get; set; }
        public int pengguna_dpt_l { get; set; }
        public int pengguna_dpt_p { get; set; }
        public int pengguna_dptb_j { get; set; }
        public int pengguna_dptb_l { get; set; }
        public int pengguna_dptb_p { get; set; }
        public int suara_tidak_sah { get; set; }
        public int pengguna_total_j { get; set; }
        public int pengguna_total_l { get; set; }
        public int pengguna_total_p { get; set; }
        public int pengguna_non_dpt_j { get; set; }
        public int pengguna_non_dpt_l { get; set; }
        public int pengguna_non_dpt_p { get; set; }
    }
    [NotMapped]
    public class Chart
    {
        //public object @null { get; set; }

        [JsonPropertyName("100025")]
        public int _100025 { get; set; }

        [JsonPropertyName("100026")]
        public int _100026 { get; set; }

        [JsonPropertyName("100027")]
        public int _100027 { get; set; }
    }
}

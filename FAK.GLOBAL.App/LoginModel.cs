using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    [JsonObject("data")]
    public class LoginModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string groupName { get; set; }
        public string token { get; set; }
        public DateTime? expiry { get; set; }
        public string sts { get; set; }
        public bool expired { get; set; }
    }
}

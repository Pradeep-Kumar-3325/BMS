using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Domain
{
    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string State { get; set; }

        public int Pincode { get; set; }

        public string Country { get; set; }
    }
}

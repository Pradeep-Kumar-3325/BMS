using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Domain
{
    public class Branch
    {
        public double BranchId { get; set; }

        public string BankName { get; set; }

        public string Name { get; set; }

        public string IFSCCode { get; set; }

        public Address Address { get; set; }
    }
}

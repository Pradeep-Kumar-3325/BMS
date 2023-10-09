using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Domain
{
    public class Transaction
    {
        public double TransactionId { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime Datetime { get; set; }

        public double AccountNumber { get; set; }
    }
}

using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Domain
{
    public class FailedTransactionLog
    {
        public double FailedTransactionLogId { get; set; }

        public double TransactionId { get; set; }

        public string ErrorType { get; set; }

        public DateTime DateTime { get; set; }
    }
}

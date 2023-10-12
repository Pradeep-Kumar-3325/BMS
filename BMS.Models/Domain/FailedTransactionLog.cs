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
        public long FailedTransactionLogId { get; set; }

        public long TransactionId { get; set; }

        public string ErrorType { get; set; }

        public DateTime DateTime { get; set; }
    }
}

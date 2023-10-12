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
        /// <summary>
        /// Auto Generated for sepecific transaction
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// type would be 0 = withdraw, 1= deposite
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        /// withdow amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date time of transaction
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Account Number from which amount will be deducted
        /// </summary>
        public long AccountNumber { get; set; }
    }
}

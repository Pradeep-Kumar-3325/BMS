using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    public class TransactionDetail
    {
        /// <summary>
        /// Amount will be proceed. In case of deposit amount can not be more than 10000
        /// </summary>
        [Required(ErrorMessage = "Amount field is required.")]
        public Decimal Amount { get; set; } = new Decimal(0);

        /// <summary>
        /// Account Number from which amount will be deducted
        /// </summary>
        [Required(ErrorMessage = "Account Number field is required.")]
        public double AccountNumber { get; set; }
    }
}

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
        [Required(ErrorMessage = "Amount field is required.")]
        public Decimal Amount { get; set; } = new Decimal(0);

        [Required(ErrorMessage = "Account Number field is required.")]
        public double AccountNumber { get; set; }
    }
}

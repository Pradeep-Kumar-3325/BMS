using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    public class TransactionWithdrawDetail : TransactionDetail
    {
        [Required(ErrorMessage = "UserName field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        public string Password { get; set; }
    }
}

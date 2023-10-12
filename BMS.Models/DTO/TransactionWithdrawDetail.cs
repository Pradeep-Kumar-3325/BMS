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
        /// <summary>
        /// User Name is must and should be user name which you prvoide during account creation
        /// </summary>
        [Required(ErrorMessage = "UserName field is required.")]
        public string UserName { get; set; }

        /// <summary>
        /// Password would be "Welcome@123"
        /// </summary>
        [Required(ErrorMessage = "Password field is required.")]
        public string Password { get; set; }
    }
}

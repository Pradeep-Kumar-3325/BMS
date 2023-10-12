using BMS.Models.Domain;
using BMS.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    /// <summary> Request for create account </summary>
    public  class AccountDetail
    {
        /// <summary> Account type would be saving, current and salary </summary>
        [Required(ErrorMessage = "Account Type field is required.")]
        public string AccountType { get; set; } // use PascalCase for property naming

        /// <summary>
        /// Starting Balance should be 100 or more
        /// </summary>
        [Required(ErrorMessage = "Account Type field is required.")]
        public Decimal Balance { get; set; }

        /// <summary>
        /// Customer Detail
        /// </summary>
        public CustomerDetail Customer { get; set; }

        public BranchDetail BranchDetail { get; set; }

    }
}

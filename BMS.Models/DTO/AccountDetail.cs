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
    public  class AccountDetail
    {
        [Required(ErrorMessage = "Account Type field is required.")]
        public string AccountType { get; set; } // use PascalCase for property naming

        [Required(ErrorMessage = "Account Type field is required.")]
        public Decimal Balance { get; set; }

        public CustomerDetail Customer { get; set; }

        public BranchDetail BranchDetail { get; set; }

    }
}

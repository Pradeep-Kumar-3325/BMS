using BMS.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    public class BranchDetail
    {
        [Required(ErrorMessage = "Branch Name field is required.")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Bank field is required.")]
        public string BankName { get; set; }

       // public Address Address { get; set; }
    }
}

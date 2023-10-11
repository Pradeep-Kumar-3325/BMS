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
        /// <summary>
        /// branch name should be crpf camp
        /// </summary>
        [Required(ErrorMessage = "Branch Name field is required.")]
        public string BranchName { get; set; }

        /// <summary>
        /// bank name should be sbi
        /// </summary>
        [Required(ErrorMessage = "Bank field is required.")]
        public string BankName { get; set; }

       // public Address Address { get; set; }
    }
}

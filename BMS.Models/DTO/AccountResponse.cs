using BMS.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    public class AccountResponse
    {
        public Account Account { get; set; }

        public string ValidationMessage { get; set; }
    }
}

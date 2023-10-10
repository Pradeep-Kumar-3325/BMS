using BMS.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.DTO
{
    public class TransactionResponse
    {
        public Transaction Transaction { get; set; }

        public string ValidationMessage { get; set; }
    }
}

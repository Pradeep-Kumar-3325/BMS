using BMS.Models.Enum;

namespace BMS.Models.Domain
{
    public class Account
    {
        // Account Number
        public double AccountNumber { get; set; }

        public double CustomerId { get; set; }

        public double BranchId { get; set; }

        public decimal Balance { get; set; }

        public AccountType Type { get; set; }

        public DateTime OpeningDate { get; set; }

    }
}
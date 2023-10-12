using BMS.Models.Enum;

namespace BMS.Models.Domain
{
    public class Account
    {
        // Account Number
        public long AccountNumber { get; set; }

        public long CustomerId { get; set; }

        public long BranchId { get; set; }

        public decimal Balance { get; set; }

        public AccountType Type { get; set; }

        public DateTime OpeningDate { get; set; }

    }
}
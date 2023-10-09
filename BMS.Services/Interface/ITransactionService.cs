using BMS.Models.Domain;
using BMS.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Interface
{
    public interface ITransactionService
    {
        Task<Transaction> Withdraw(TransactionWithdrawDetail transactionDetail);

        Task<Transaction> Deposit(TransactionDetail transactionDetail);
    }
}

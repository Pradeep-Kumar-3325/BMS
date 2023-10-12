using BMS.Models.Domain;
using BMS.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Interface
{
    public interface IAccountService
    {
        Task<AccountResponse> Create(AccountDetail accountDetail);

        Task<Account> Get(long accountNumber);

        Task<bool> Delete(long accountNumber);

        Task<Account> Update(Account account, long accountNumber);
    }
}

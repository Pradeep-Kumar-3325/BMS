﻿using BMS.Models.Domain;
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
        Task<Account> Create(AccountDetail accountDetail);

        Task<Account> Get(double accountNumber);

        Task<bool> Delete(double accountNumber);

        Task<Account> Update(Account account, double accountNumber);
    }
}

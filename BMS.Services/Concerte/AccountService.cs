using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Models.Enum;
using BMS.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace BMS.Services.Concerte
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly ICustomerService customerService;
        private readonly IBranchService branchService;
        private IRepository<Account> repoAccount;
        private readonly IConfiguration configuration;

        // D :- Dependency Inversion  of SOLID :- by passing dependency from constructor
        // S :- Single Responsibility of SOLID :- Divide repsonsibility between customer, account and branch service
        public AccountService(ILogger<AccountService> logger, IConfiguration configuration, ICustomerService customer, IBranchService branch, IRepository<Account> repoAccount)
        {
            this._logger = logger;
            this.customerService = customer;
            this.branchService = branch;
            this.configuration = configuration;
            //// repoAccount = new Repository<Account>(_logger);
            this.repoAccount = repoAccount;
        }

        /// <summary>
        /// Create New Account.
        /// </summary>
        /// <param name="accountDetail"></param>
        /// <returns></returns>
        public async Task<AccountResponse> Create(AccountDetail accountDetail)
        {
            try
            {
                AccountResponse response = new AccountResponse();
                var minAmount = configuration["Rule:MinAmount"];
                if (string.IsNullOrEmpty(minAmount))
                    _logger.LogInformation($"Configuration of MinAmount is missing");

                if (accountDetail.Balance < Convert.ToDecimal(minAmount))
                {
                    response.ValidationMessage = "An account cannot have less than $100 at any time in an account!";
                    return response;
                    //throw new Exception("An account cannot have less than $100 at any time in an account!");
                }

                //// Here we can use unit of work and repository of customer and branch
               //// to create user or get branch detail for account
                var customer = await this.customerService.CreateOrGet(accountDetail.Customer);
                var branch = await this.branchService.GetByName(accountDetail.BranchDetail.BranchName, accountDetail.BranchDetail.BankName);

                if (branch == null)
                {
                    response.ValidationMessage = "Not able to find Branch! Please enter sbi as bank name and crpf camp as branch name";
                    return response;
                    //throw new Exception("An account cannot have less than $100 at any time in an account!");
                }

                var account = new Account
                {
                    AccountNumber = new Random().Next(0, Int32.MaxValue),
                    CustomerId = customer.CustomerId,
                    BranchId = branch.BranchId,
                    Balance=accountDetail.Balance,
                    OpeningDate = DateTime.Now,
                    Type = (AccountType)Enum.Parse(typeof(AccountType), accountDetail.AccountType, true) 
                };

                var createdAccount = this.repoAccount.Insert(account, account.AccountNumber);
                response.Account = createdAccount;
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");
                throw;
            }
        }

        /// <summary>
        /// Get Account detail by AccountName
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public async Task<Account> Get(double accountNumber) // use camelCase for paremetr naming
        {
            try
            {
                var account = repoAccount.Get(accountNumber);

                return await Task.FromResult(account);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                throw;
            }
        }
        public async Task<bool> Delete(double accountNumber)
        {
            try
            {
                var deleted = repoAccount.Delete(accountNumber);

                return await Task.FromResult(deleted);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                throw;
            }
        }

        public async Task<Account> Update(Account account, double accountNumber)
        {
            try
            {
                var updated = repoAccount.Update(account, accountNumber);

                return await Task.FromResult(updated);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Update :- {ex}");
                throw;
            }
        }
    }
}
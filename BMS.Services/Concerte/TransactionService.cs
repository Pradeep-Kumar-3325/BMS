using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Models.Enum;
using BMS.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BMS.Services.Concerte
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly IAccountService accountService;
        private readonly ICustomerService customerService;
        private IRepository<Transaction> repoTransaction;
        private IRepository<FailedTransactionLog> repoFailedTransactionLog;
        private readonly IConfiguration configuration;
        public TransactionService(ILogger<TransactionService> logger, IConfiguration configuration,
            IAccountService account,
            ICustomerService customer,
            IRepository<Transaction> repoTransaction, 
            IRepository<FailedTransactionLog> repoFailedTransactionLog)
        {
            this._logger = logger;
            this.accountService = account;
            this.customerService = customer;
            this.repoTransaction = repoTransaction;
            this.configuration = configuration;
            this.repoFailedTransactionLog = repoFailedTransactionLog;
        }

        /// <summary>
        /// Deposit the money to given account number
        /// </summary>
        /// <param name="transactionDetail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TransactionResponse> Deposit(TransactionDetail transactionDetail)
        {
            Transaction createdTransaction = null;
            try
            {
                TransactionResponse response = new TransactionResponse();
                var account = await accountService.Get(transactionDetail.AccountNumber);

                if (account == null)
                {
                    throw new Exception("An account does not exist!");
                }

                if (!ValidateDeposit(account, transactionDetail, response))
                {
                    return response;
                }

                decimal finalAmount = account.Balance + transactionDetail.Amount;

                createdTransaction = this.CreateTransaction(transactionDetail, TransactionType.Deposite);

                if (createdTransaction != null)
                {
                    account.Balance = finalAmount;
                    await accountService.Update(account, transactionDetail.AccountNumber);
                }

                response.Transaction = createdTransaction;

                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");

                if (createdTransaction == null)
                {
                    createdTransaction = this.CreateTransaction(transactionDetail, TransactionType.Deposite);
                }

                if (createdTransaction != null)
                {
                    var transactionFailed = new FailedTransactionLog
                    {
                        FailedTransactionLogId = new Random().Next(0, Int32.MaxValue),
                        TransactionId = createdTransaction.TransactionId,
                        ErrorType = TransactionType.Deposite.ToString(),
                        DateTime = DateTime.Now
                    };

                    var createdTransactionFailed = this.repoFailedTransactionLog.Insert(transactionFailed, transactionFailed.FailedTransactionLogId);
                }

                throw;
            }
        }

        /// <summary>
        /// Withdraw the money to given account number
        /// </summary>
        /// <param name="transactionDetail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TransactionResponse> Withdraw(TransactionWithdrawDetail transactionDetail)
        {
            Transaction createdTransaction = null;
            try
            {
                TransactionResponse response = new TransactionResponse();

                var vaildUser = this.customerService.ValidUser(transactionDetail.UserName, transactionDetail.Password);

                if(!vaildUser)
                {
                    response.ValidationMessage = "User Name is not vaild! Please enter Welcome@123 in password";
                    return response;
                    //throw new Exception("User is not vaild!");
                }

                var account = await accountService.Get(transactionDetail.AccountNumber);

                if (account == null)
                {
                    response.ValidationMessage = "An account does not exist!";
                }

                if (!ValidateWithdraw(account, transactionDetail, response))
                {
                    return response;
                   // throw new Exception("Error in Withdraw!");
                }

                decimal finalAmount = account.Balance - transactionDetail.Amount;

                //var transaction = new Transaction
                //{
                //    TransactionId = new Random().Next(0, Int32.MaxValue),
                //    AccountNumber = transactionDetail.AccountNumber,
                //    Amount = transactionDetail.Amount,
                //    Type = TransactionType.Withdraw,
                //    Datetime = DateTime.Now
                //};

                //var createdTransaction = this.repoTransaction.Insert(transaction, transaction.TransactionId);

                createdTransaction = this.CreateTransaction(transactionDetail, TransactionType.Withdraw);

                if (createdTransaction != null)
                {
                    account.Balance = finalAmount;
                    await accountService.Update(account, transactionDetail.AccountNumber);
                }

                response.Transaction = createdTransaction;

                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");

                if (createdTransaction == null)
                {
                    createdTransaction = this.CreateTransaction(transactionDetail, TransactionType.Withdraw);
                }

                if (createdTransaction != null)
                {
                    var transactionFailed = new FailedTransactionLog
                    {
                        FailedTransactionLogId = new Random().Next(0, Int32.MaxValue),
                        TransactionId = createdTransaction.TransactionId,
                        ErrorType = TransactionType.Withdraw.ToString(),
                        DateTime = DateTime.Now
                    };

                    var createdTransactionFailed = this.repoFailedTransactionLog.Insert(transactionFailed, transactionFailed.FailedTransactionLogId);
                }

                throw;
            }
        }

        private Transaction CreateTransaction(TransactionDetail transactionDetail, TransactionType type)
        {
            var transaction = new Transaction
            {
                TransactionId = new Random().Next(0, Int32.MaxValue),
                AccountNumber = transactionDetail.AccountNumber,
                Amount = transactionDetail.Amount,
                Type = type,
                Datetime = DateTime.Now
            };

            var createdTransaction = this.repoTransaction.Insert(transaction, transaction.TransactionId);

            return createdTransaction;
        }

        private bool ValidateWithdraw(Account account, TransactionWithdrawDetail transactionDetail, TransactionResponse response)
        {
            try
            {
                string message = string.Empty;
                decimal finalAmount = account.Balance - transactionDetail.Amount;

                var minAmount = configuration["Rule:MinAmount"];
                if (string.IsNullOrEmpty(minAmount))
                {
                    message = $"Configuration of MinAmount is missing";
                    _logger.LogError(message);
                    throw new Exception(message);

                }

                if (finalAmount < Convert.ToDecimal(minAmount))
                {
                     message = $"An account cannot have less than {minAmount} at any time in an account!";
                    _logger.LogError(message);
                    response.ValidationMessage = message;
                    return false;
                }

                var minPercent = configuration["Rule:MinPercent"];
                if (string.IsNullOrEmpty(minPercent))
                {
                    message = $"Configuration of MinPercent is missing";
                    _logger.LogError(message);
                    throw new Exception(message);
                }

                decimal amountpercent = (Convert.ToDecimal(minPercent) / 100) * account.Balance;

                if (transactionDetail.Amount > amountpercent)
                {
                     message = $"Cannot withdraw more than {minPercent} % of their total balance from an account in a single transaction";
                    _logger.LogError(message);
                    response.ValidationMessage = message;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in validate :- {ex}");
                throw;
            }

            return true;
        }

        private bool ValidateDeposit(Account account, TransactionDetail transactionDetail, TransactionResponse response)
        {
            try
            {
                string message = string.Empty;
                var maxDeposit = configuration["Rule:MaxDeposit"];
                if (string.IsNullOrEmpty(maxDeposit))
                {
                     message = $"Configuration of MaxDeposit is missing";
                    _logger.LogError(message);
                    throw new Exception(message);
                }

                if (transactionDetail.Amount > Convert.ToDecimal(maxDeposit))
                {
                     message = $"Cannot deposit more than {maxDeposit} in a single transaction";
                    _logger.LogError(message);
                    response.ValidationMessage = message;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in validate :- {ex}");
                throw;
            }

            return true;
        }
    }
}
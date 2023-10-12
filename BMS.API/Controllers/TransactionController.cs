using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BMS.API.Controllers
{
    /// <summary>
    /// We Can use well known Authentication and Authorization process like oauth 2 and 2 way authenication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transaction;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService _transaction)
        {
            _logger = logger;
            this._transaction = _transaction;
        }

        /// <summary>
        /// Withdraw Amount from specific account
        /// </summary>
        /// <remarks>
        /// Please enter password Welcome@123 and user name along with other detail for withdraw
        /// </remarks>
        /// <param name="transactionDetail"> transaction detail</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction", typeof(TransactionResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [HttpPost]
        [Route("/api/Withdraw")]
        public async Task<IActionResult> Withdraw(TransactionWithdrawDetail transactionDetail)
        {
            try
            {
                return this.Ok(await this._transaction.Withdraw(transactionDetail));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Withdraw :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }

        /// <summary>
        /// Deposit the Amount to specific Account
        /// </summary>
        /// <param name="transactionDetail"></param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction", typeof(TransactionResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [HttpPost]
        [Route("/api/Deposit")]
        public async Task<IActionResult> Deposit(TransactionDetail transactionDetail)
        {
            try
            {
                return this.Ok(await this._transaction.Deposit(transactionDetail));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Deposit :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }
    }
}

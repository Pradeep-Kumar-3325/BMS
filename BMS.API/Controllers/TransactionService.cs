using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transaction;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService _transaction)
        {
            _logger = logger;
            _transaction = _transaction;
        }

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

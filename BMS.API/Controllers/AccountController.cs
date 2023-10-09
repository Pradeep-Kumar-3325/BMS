using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _account;

        public AccountController(ILogger<AccountController> logger, IAccountService account)
        {
            _logger = logger;
            _account = account;
        }

        [HttpGet]
        [Route("/api/Get")]
        public async Task<IActionResult> Get(double Id)
        {
            try
            {
                return this.Ok(await this._account.Get(Id));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }

        [HttpPost]
        [Route("/api/Create")]
        public async Task<IActionResult> Create(AccountDetail accountDetail)
        {
            try
            {
                return this.Ok(await this._account.Create(accountDetail));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }

        [HttpDelete]
        [Route("/api/Delete")]
        public async Task<IActionResult> Delete(double accountNumber)
        {
            try
            {
                return this.Ok(await this._account.Delete(accountNumber));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }


    }
}

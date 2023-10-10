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
                this._logger.LogError($"Error in Create :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }

        [HttpGet]
        [Route("/api/Get")]
        public async Task<IActionResult> Get(double accountNumber)
        {
            try
            {
                var response = await this._account.Get(accountNumber);

                if (response == null)
                    return this.Ok("No Account exists for given account number!");

                return this.Ok(response);
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
                var response = await this._account.Delete(accountNumber);

                if (!response)
                    return this.Ok("No Account exists for given account number!");

                return this.Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while processing data!");
            }
        }


    }
}

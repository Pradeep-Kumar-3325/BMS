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
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _account;

        public AccountController(ILogger<AccountController> logger, IAccountService account)
        {
            _logger = logger;
            _account = account;
        }

        /// <summary>
        /// Create Account and Customer if customer is not exist otherwise
        /// create account for same customer which have same username and email
        /// </summary>
        /// <remarks>
        /// Account type would be saving, current and salary
        /// branch name should be crpf camp and bank name should be sbi
        /// </remarks>
        /// <param name="accountDetail">Account Details</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Account", typeof(AccountResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [HttpPost]
        [Route("/api/Create")]
        public async Task<IActionResult> Create([FromBody]AccountDetail accountDetail)
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

        /// <summary>
        /// Get Account detail of given account number
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK, "Account", typeof(Account))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
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

        /// <summary>
        /// Get Delete detail of given account number
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK, "Message", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
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

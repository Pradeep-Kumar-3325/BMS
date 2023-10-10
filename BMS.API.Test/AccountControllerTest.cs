using BMS.API.Controllers;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BMS.API.Test
{
    public  class AccountControllerTest
    {
        private readonly Mock<ILogger<AccountController>> logger;

        private readonly Mock<IAccountService> accountService;

        AccountController account = null;

        //Initialization
        public AccountControllerTest()
        {
            logger = new Mock<ILogger<AccountController>>();
            accountService = new Mock<IAccountService>();

            account = new AccountController(logger.Object, accountService.Object);
        }

        [Fact]
        public async Task Create_Account_When_Return_Data_From_AccountService()
        {
            //Arrange
            AccountDetail accountDetail = new AccountDetail();

            Account result = new Account
            {
                AccountNumber = 1234,
                CustomerId = 12
            };

            AccountResponse response = new AccountResponse
            {
                Account = result
            };

            accountService.Setup(x => x.Create(accountDetail))
               .ReturnsAsync(response);

            //Act
            IActionResult actionResult = await account.Create(accountDetail);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal((contentResult.Value as  AccountResponse).Account.AccountNumber, result.AccountNumber);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Create_Throw_Exception_When_AccountService_Throw_Exception()
        {
            //Arrange
            accountService.Setup(x => x.Create(It.IsAny<AccountDetail>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await account.Create(It.IsAny<AccountDetail>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 500;
            string exception = "Error while processing data!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Get_Account_When_Return_Data_From_AccountService()
        {
            //Arrange
            
            Account result = new Account
            {
                AccountNumber = 1234,
                CustomerId = 12
            };


            accountService.Setup(x => x.Get(It.IsAny<double>()))
               .ReturnsAsync(result);

            //Act
            IActionResult actionResult = await account.Get(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal((contentResult.Value as Account).AccountNumber, result.AccountNumber);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Get_Account_When_Return_Null_From_AccountService()
        {
            //Arrange

            Account result = null;
            accountService.Setup(x => x.Get(It.IsAny<double>()))
               .ReturnsAsync(result);

            //Act
            IActionResult actionResult = await account.Get(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal(contentResult.Value.ToString(), "No Account exists for given account number!");
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Get_Throw_Exception_When_AccountService_Throw_Exception()
        {
            //Arrange
            accountService.Setup(x => x.Get(It.IsAny<double>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await account.Get(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 500;
            string exception = "Error while processing data!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Delete_Account_When_Return_Data_From_AccountService()
        {
            //Arrange
            accountService.Setup(x => x.Delete(It.IsAny<double>()))
               .ReturnsAsync(true);

            //Act
            IActionResult actionResult = await account.Delete(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal(contentResult.Value.ToString(), "Deleted Successfully");
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Delete_Account_When_Return_False_From_AccountService()
        {
            //Arrange
            accountService.Setup(x => x.Delete(It.IsAny<double>()))
               .ReturnsAsync(false);

            //Act
            IActionResult actionResult = await account.Delete(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal(contentResult.Value.ToString(), "No Account exists for given account number!");
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Delete_Throw_Exception_When_AccountService_Throw_Exception()
        {
            //Arrange
            accountService.Setup(x => x.Delete(It.IsAny<double>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await account.Delete(It.IsAny<double>());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 500;
            string exception = "Error while processing data!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }
    }
}

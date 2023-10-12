using BMS.API.Controllers;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Transactions;

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
        public async Task Create_When_TransactionDetail_ISNULL_THEN_Throw_BadRequest()
        {
            //Arrange && Act
            IActionResult actionResult = await account.Create(null);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 400;
            string exception = "Please Provide AccountDetail!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
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
            IActionResult actionResult = await account.Create(new AccountDetail());
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
        public async Task Get_When_TransactionDetail_ISNULL_THEN_Throw_BadRequest()
        {
            //Arrange && Act
            IActionResult actionResult = await account.Get(0);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 400;
            string exception = "Please Provide Valid Account Number!";
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


            accountService.Setup(x => x.Get(It.IsAny<long>()))
               .ReturnsAsync(result);

            //Act
            IActionResult actionResult = await account.Get(1234);
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
            accountService.Setup(x => x.Get(It.IsAny<long>()))
               .ReturnsAsync(result);

            //Act
            IActionResult actionResult = await account.Get(123);
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
            accountService.Setup(x => x.Get(It.IsAny<long>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await account.Get(123);
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
        public async Task Delete_When_TransactionDetail_ISNULL_THEN_Throw_BadRequest()
        {
            //Arrange && Act
            IActionResult actionResult = await account.Delete(0);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 400;
            string exception = "Please Provide Valid Account Number!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Delete_Account_When_Return_Data_From_AccountService()
        {
            //Arrange
            accountService.Setup(x => x.Delete(123))
               .ReturnsAsync(true);

            //Act
            IActionResult actionResult = await account.Delete(123);
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
            accountService.Setup(x => x.Delete(It.IsAny<long>()))
               .ReturnsAsync(false);

            //Act
            IActionResult actionResult = await account.Delete(1);
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
            accountService.Setup(x => x.Delete(It.IsAny<long>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await account.Delete(1);
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

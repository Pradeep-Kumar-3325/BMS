using BMS.API.Controllers;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BMS.API.Test
{
    public class TransactionControllerTest
    {
        private readonly Mock<ILogger<TransactionController>> logger;

        private readonly Mock<ITransactionService> transService;

        TransactionController transaction = null;

        //Initialization
        public TransactionControllerTest()
        {
            logger = new Mock<ILogger<TransactionController>>();
            transService = new Mock<ITransactionService>();

            transaction = new TransactionController(logger.Object, transService.Object);
        }

        [Fact]
        public async Task Withdraw_When_TransactionDetail_ISNULL_THEN_Throw_BadRequest()
        {
            //Arrange && Act
            IActionResult actionResult = await transaction.Withdraw(null);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 400;
            string exception = "Please Provide Transaction Details!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Withdraw_Account_When_Return_Data_From_TransactionSeeervice()
        {
            //Arrange
            TransactionWithdrawDetail transctionDetail = new TransactionWithdrawDetail();

            Transaction result = new Transaction
            {
                AccountNumber = 1234
            };

            TransactionResponse response = new TransactionResponse
            {
                Transaction = result
            };

            transService.Setup(x => x.Withdraw(It.IsAny<TransactionWithdrawDetail>()))
               .ReturnsAsync(response);

            //Act
            IActionResult actionResult = await transaction.Withdraw(new TransactionWithdrawDetail());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal((contentResult.Value as TransactionResponse).Transaction.AccountNumber, result.AccountNumber);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Withdarw_Throw_Exception_When_AccountService_Throw_Exception()
        {
            //Arrange
            transService.Setup(x => x.Withdraw(It.IsAny<TransactionWithdrawDetail>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await transaction.Withdraw(new TransactionWithdrawDetail());
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
        public async Task Deposit_When_TransactionDetail_ISNULL_THEN_Throw_BadRequest()
        {
            //Arrange && Act
            IActionResult actionResult = await transaction.Deposit(null);
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 400;
            string exception = "Please Provide Transaction Details!";
            Assert.Equal(contentResult.Value.ToString(), exception);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Deposit_Account_When_Return_Data_From_Transactionervice()
        {
            //Arrange
            Transaction result = new Transaction
            {
                AccountNumber = 1234
            };

            TransactionResponse response = new TransactionResponse
            {
                Transaction = result
            };

            transService.Setup(x => x.Deposit(It.IsAny<TransactionDetail>()))
               .ReturnsAsync(response);

            //Act
            IActionResult actionResult = await transaction.Deposit(new TransactionDetail());
            var contentResult = actionResult as ObjectResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            int exceptedStatusCode = 200;
            Assert.Equal((contentResult.Value as TransactionResponse).Transaction.AccountNumber, result.AccountNumber);
            Assert.Equal(contentResult.StatusCode, exceptedStatusCode);
        }

        [Fact]
        public async Task Deposit_Throw_Exception_When_AccountService_Throw_Exception()
        {
            //Arrange
            transService.Setup(x => x.Deposit(It.IsAny<TransactionDetail>()))
               .ThrowsAsync(new Exception());

            //Act
            IActionResult actionResult = await transaction.Deposit(new TransactionDetail());
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

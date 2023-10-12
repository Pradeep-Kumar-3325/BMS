using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Concerte;
using BMS.Services.Interface;
using Castle.Core.Resource;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BMS.Services.Test
{
    public class TransactionServiceTest
    {
        private readonly Mock<ILogger<TransactionService>> logger;
        private readonly Mock<IConfiguration> config;

        private readonly Mock<ICustomerService> customerService;
        private readonly Mock<IAccountService> accountService;
        private readonly Mock<IRepository<Transaction>> data;
        private readonly Mock<IRepository<FailedTransactionLog>> dataFailed;

        TransactionService transactionService = null;

        //Initialization
        public TransactionServiceTest()
        {
            logger = new Mock<ILogger<TransactionService>>();
            config = new Mock<IConfiguration>();
            customerService = new Mock<ICustomerService>();
            accountService = new Mock<IAccountService>();
            data = new Mock<IRepository<Transaction>>();
            dataFailed = new Mock<IRepository<FailedTransactionLog>>();
            transactionService = new TransactionService(logger.Object, config.Object, accountService.Object, customerService.Object,  data.Object, dataFailed.Object);
        }

        /*
         * The below Test Method cover 90% of all branches and line of code 
         * because created all possible data in setupData Method 
         */
        [Fact]
        public async Task Withdraw_Transaction_When_Valid_TransactionDetail()
        {
            //Arrange
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");
            config.Setup(x => x["Rule:MinPercent"]).Returns("90");

            this.setupData();
            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 100,
                UserName ="pradeep",
                Password = "Welcome@123"
            };

            //Act
             var result = await transactionService.Withdraw(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Transaction.AccountNumber);
            Assert.Equal(100, result.Transaction.Amount);
        }

        [Fact]
        public async Task Withdraw_Transaction_When_User_Is_Valid_Return_BusinessRule_Validation()
        {
            //Arrange
            customerService.Setup(x => x.ValidUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 100,
                UserName = "pradeep",
                Password = "Welcome@123"
            };

            //Act
            var result = await transactionService.Withdraw(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("User Name is not vaild! Please enter Welcome@123 in password", result.ValidationMessage);
        }

        [Fact]
        public async Task Withdraw_Transaction_When_Account_Is_NoExist_Return_BusinessRule_Validation()
        {
            //Arrange
            customerService.Setup(x => x.ValidUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 100,
                UserName = "pradeep",
                Password = "Welcome@123"
            };

            Account account = null;

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            //Act
            var result = await transactionService.Withdraw(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("An account does not exist!", result.ValidationMessage);
        }

        [Fact]
        public async Task Withdraw_Transaction_When_Account_Balance_Less100_Return_BusinessRule_Validation()
        {
            //Arrange
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");

            customerService.Setup(x => x.ValidUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Account account = new Account
            {
                AccountNumber = 1,
                Balance = 100
            };

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 100
            };

            //Act
            var result = await transactionService.Withdraw(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("An account cannot have less than 100 at any time in an account!", result.ValidationMessage);
        }

        [Fact]
        public async Task Withdraw_Transaction_When_More_Than_90Percent_Return_BusinessRule_Validation()
        {
            //Arrange
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");
            config.Setup(x => x["Rule:MinPercent"]).Returns("90");

            customerService.Setup(x => x.ValidUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Account account = new Account
            {
                AccountNumber = 1,
                Balance = 10000
            };

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 9001
            };

            //Act
            var result = await transactionService.Withdraw(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Cannot withdraw more than 90 % of their total balance from an account in a single transaction", result.ValidationMessage);
        }

        [Fact]
        public async Task Withdraw_Transaction_Throw_Exception_When_Configuration_Missing()
        {
            //Arrange
            this.setupData();
            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 100,
                UserName = "pradeep",
                Password = "Welcome@123"
            };

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(async () => await transactionService.Withdraw(detail));
        }

        [Fact]
        public async Task Deposit_Transaction_When_Valid_TransactionDetail()
        {
            //Arrange
            config.Setup(x => x["Rule:MaxDeposit"]).Returns("10000");

            this.setupDepositData();
            TransactionDetail detail = new TransactionDetail
            {
                AccountNumber = 1,
                Amount = 100
            };

            //Act
            var result = await transactionService.Deposit(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Transaction.AccountNumber);
            Assert.Equal(100, result.Transaction.Amount);
        }

        [Fact]
        public async Task Deposit_Transaction_When_Account_Is_NoExist_Return_BusinessRule_Validation()
        {
            //Arrange

            TransactionDetail detail = new TransactionDetail
            {
                AccountNumber = 1,
                Amount = 100,
            };

            Account account = null;

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            //Act
            var result = await transactionService.Deposit(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("An account does not exist!", result.ValidationMessage);
        }
       
        [Fact]
        public async Task Deposit_Transaction_When_More_Than_10000_Return_BusinessRule_Validation()
        {
            //Arrange
            config.Setup(x => x["Rule:MaxDeposit"]).Returns("10000");

            Account account = new Account
            {
                AccountNumber = 1,
                Balance = 10000
            };

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            TransactionWithdrawDetail detail = new TransactionWithdrawDetail
            {
                AccountNumber = 1,
                Amount = 10001
            };

            //Act
            var result = await transactionService.Deposit(detail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Cannot deposit more than 10000 in a single transaction", result.ValidationMessage);
        }

        [Fact]
        public async Task Deposit_Transaction_Throw_Exception_When_Configuration_Missing()
        {
            //Arrange
            this.setupDepositData();
            TransactionDetail detail = new TransactionDetail
            {
                AccountNumber = 1,
                Amount = 100,
            };

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(async () => await transactionService.Deposit(detail));
        }

        private void setupData()
        {
            customerService.Setup(x => x.ValidUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Account account = new Account
            {
                AccountNumber = 1,
                Balance=10000
            };

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            Transaction transaction = new Transaction
            {
                AccountNumber = 1,
                TransactionId = 1,
                Amount = 100
            };

            data.Setup(x => x.Insert(It.IsAny<Transaction>(), It.IsAny<double>()))
                .Returns(transaction);
        }

        private void setupDepositData()
        {
            Account account = new Account
            {
                AccountNumber = 1,
                Balance = 10000
            };

            accountService.Setup(x => x.Get(It.IsAny<double>()))
                .ReturnsAsync(account);

            Transaction transaction = new Transaction
            {
                AccountNumber = 1,
                TransactionId = 1,
                Amount = 100
            };

            data.Setup(x => x.Insert(It.IsAny<Transaction>(), It.IsAny<double>()))
                .Returns(transaction);
        }
    }
}
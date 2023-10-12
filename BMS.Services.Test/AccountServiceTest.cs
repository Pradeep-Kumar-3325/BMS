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
using System.Security.Principal;

namespace BMS.Services.Test
{
    public class AccountServiceTest
    {
        private readonly Mock<ILogger<AccountService>> logger;
        private readonly Mock<IConfiguration> config;

        private readonly Mock<ICustomerService> customerService;
        private readonly Mock<IBranchService> branchService;
        private readonly Mock<IRepository<Account>> data;

        AccountService accountService = null;

        //Initialization
        public AccountServiceTest()
        {
            logger = new Mock<ILogger<AccountService>>();
            config = new Mock<IConfiguration>();
            customerService = new Mock<ICustomerService>();
            branchService = new Mock<IBranchService>();
            data = new Mock<IRepository<Account>>();
            accountService = new AccountService(logger.Object, config.Object, customerService.Object, branchService.Object, data.Object);
        }

        /*
         * The below Test Method cover 90% of all branches and line of code 
         * because created all possible data in setupData Method 
         */
        [Fact]
        public async Task Create_Account_When_Valid_AccountDetail()
        {
            //Arrange
            this.setupData();
            AccountDetail accountDetail = new AccountDetail
            {
                Balance = 100,
                AccountType ="Saving",
                BranchDetail = new BranchDetail
                {
                    BankName = "sbi",
                    BranchName = "crpf camp"
                }
            };

            //Act
             var result = await accountService.Create(accountDetail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Account.AccountNumber, 12345);
        }

        [Fact]
        public async Task Create_Account_When_Balance_Less_100_Return_BusinessRule_Validation()
        {
            //Arrange
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");
            AccountDetail accountDetail = new AccountDetail
            {
                Balance = 0,
            };

            //Act
            var result = await accountService.Create(accountDetail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.ValidationMessage, "An account cannot have less than $100 at any time in an account!");
        }

        [Fact]
        public async Task Create_Account_When_Branch_NotFound_Return_BusinessRule_Validation()
        {
            //Arrange
            this.setupBranchNullData();
            AccountDetail accountDetail = new AccountDetail
            {
                Balance = 100,
                BranchDetail = new BranchDetail
                {
                    BankName = "sbi",
                    BranchName = "crpf camp"
                }
            };

            //Act
            var result = await accountService.Create(accountDetail);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.ValidationMessage, "Not able to find Branch! Please enter sbi as bank name and crpf camp as branch name");
        }

        [Fact]
        public async Task Create_Throw_Exception_When_BranchDetail_Missing()
        {
            //Arrange
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");

            AccountDetail accountDetail = new AccountDetail
            {
                Balance = 100,
            };
            Customer customer = null;

            customerService.Setup(x => x.CreateOrGet(It.IsAny<CustomerDetail>()))
                .ReturnsAsync(customer);

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<System.NullReferenceException>(async () => await accountService.Create(accountDetail));
        }

        [Fact]
        public async Task Get_Account_When_Valid_AccountNumber()
        {
            //Arrange
            Account account = new Account
            {
                AccountNumber = 12345,
                CustomerId = 123,
                BranchId = 1,
                Balance = 100
            };

            data.Setup(x => x.Get(It.IsAny<long>()))
               .Returns(account); ;

            //Act
            var result = await accountService.Get(It.IsAny<long>());
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.AccountNumber, 12345);
        }

        [Fact]
        public async Task Get_Throw_Exception_When_Data_Throw_Exception()
        {
            //Arrange
            data.Setup(x => x.Get(It.IsAny<long>()))
               .Throws(new Exception());

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(async () => await accountService.Get(It.IsAny<long>()));
        }

        [Fact]
        public async Task Delete_Account_When_Valid_AccountNumber()
        {
            //Arrange

            data.Setup(x => x.Delete(It.IsAny<long>()))
               .Returns(true);

            //Act
            var result = await accountService.Delete(It.IsAny<long>());
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, true);
        }

        [Fact]
        public async Task Delete_Throw_Exception_When_Data_Throw_Exception()
        {
            //Arrange
            data.Setup(x => x.Delete(It.IsAny<long>()))
               .Throws(new Exception());

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(async () => await accountService.Delete(It.IsAny<long>()));
        }

        [Fact]
        public async Task Update_Account_When_Valid_AccountNumber()
        {
            //Arrange
            Account account = new Account
            {
                AccountNumber = 12345,
                CustomerId = 123,
                BranchId = 1,
                Balance = 100
            };

            data.Setup(x => x.Update(It.IsAny<Account>(), It.IsAny<long>()))
               .Returns(account); ;

            //Act
            var result = await accountService.Update(It.IsAny<Account>(), It.IsAny<long>());
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.AccountNumber, 12345);
        }

        [Fact]
        public async Task Update_Throw_Exception_When_Data_Throw_Exception()
        {
            //Arrange
            data.Setup(x => x.Update(It.IsAny<Account>(), It.IsAny<long>()))
               .Throws(new Exception());

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(async () => await accountService.Update(It.IsAny<Account>(), It.IsAny<long>()));
        }

        private void setupData()
        {
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");

            Customer customer = new Customer
            {
                CustomerId = 123
            };

            customerService.Setup(x => x.CreateOrGet(It.IsAny<CustomerDetail>()))
                .ReturnsAsync(customer);

            Branch branch = new Branch
            {
                BranchId = 1
            };

            branchService.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(branch);

            Account account = new Account
            {
                AccountNumber = 12345,
                CustomerId = customer.CustomerId,
                BranchId = branch.BranchId,
                Balance = 100
            };

            data.Setup(x => x.Insert(It.IsAny<Account>(), It.IsAny<long>()))
                .Returns(account);
        }

        private void setupBranchNullData()
        {
            config.Setup(x => x["Rule:MinAmount"]).Returns("100");

            Customer customer = new Customer
            {
                CustomerId = 123
            };

            customerService.Setup(x => x.CreateOrGet(It.IsAny<CustomerDetail>()))
                .ReturnsAsync(customer);

            Branch branch = null;

            branchService.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(branch);
        }

    }
}
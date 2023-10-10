using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Concerte;
using BMS.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

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

            data.Setup(x => x.Insert(It.IsAny<Account>(), It.IsAny<double>()))
                .Returns(account);


        }

    }
}
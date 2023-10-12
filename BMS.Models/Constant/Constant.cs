using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Constant
{
    public  class Constant
    {
        public const string Please_Provide_Valid_Account = "Please Provide Valid Account Number!";

        public const string No_Account_Exist = "No Account exists for given account number!";

        public const string Deleted_Successfully = "Deleted Successfully";

        public const string Error_Processing = "Error while processing data!";

        public const string Please_Provide_AccountDetail = "Please Provide AccountDetail!";

        public const string Please_Provide_TransactionDetail = "Please Provide Transaction Details!";

        public const string MinAmount_Missing = $"Configuration of MinAmount is missing";

        public const string Branch_Missing = "Not able to find Branch! Please enter sbi as bank name and crpf camp as branch name";

        public const string Valid_User = "User Name is not vaild! Please enter Welcome@123 in password";

        public const string MinPercent_Missing = "Configuration of MinPercent is missing";

        public const string MaxDeposit_Missing = $"Configuration of MaxDeposit is missing";

        public const string Customer_Already_Exist = "There is already customer for given username and email!";

        public const string Password = "Welcome@123";

        public const string No_Branch = "There is no Branch!";

        public const string Branch_Already = "There is more than one Branch for given branch name and bank!";
    }
}

using BMS.Models.Domain;
using BMS.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Data
{
    public class BMDDatabase
    {
        public static Dictionary<double, Customer> Customers { get; set; }

        public static Dictionary<double, Account> Accounts { get; set; }

        public static Dictionary<double, Branch> Branches { get; set; }

        public static Dictionary<double, Transaction> Transactions { get; set; }

        public static Dictionary<double, FailedTransactionLog> FailedTransactionLog { get; set; }

        static BMDDatabase()
        {
            

            if (Customers == null)
            {
                Customers = new Dictionary<double, Customer>();
            }

            if (Accounts == null)
            {
                Accounts = new Dictionary<double, Account>();
            }

            if (Transactions == null)
            {
                Transactions = new Dictionary<double, Transaction>();
            }

            if (FailedTransactionLog == null)
            {
                FailedTransactionLog = new Dictionary<double, FailedTransactionLog>();
            }
        }

        public static Dictionary<double, T> GetTable<T>()
        {
           var obj = Activator.CreateInstance<T>();
           
            switch (obj)
            {
                case Customer customer:
                    // return BMDDatabase.Customers as T;
                    var json = JsonConvert.SerializeObject(BMDDatabase.Customers);
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<double, T>>(json);
                    return dictionary;
                case Account account:
                    var jsonAccount = JsonConvert.SerializeObject(BMDDatabase.Accounts);
                    var dictionaryAccount = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonAccount);
                    return dictionaryAccount;
                case Branch branch:
                    var jsonBranch = JsonConvert.SerializeObject(BMDDatabase.Customers);
                    var dictionaryBranch = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonBranch);
                    return dictionaryBranch;
                case Transaction transaction:
                    var jsonTransaction = JsonConvert.SerializeObject(BMDDatabase.Customers);
                    var dictionaryTransaction = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonTransaction);
                    return dictionaryTransaction;
                case FailedTransactionLog transactionLog:
                    var jsonFailedTransactionLog = JsonConvert.SerializeObject(BMDDatabase.Customers);
                    var dictionaryFailedTransactionLog = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonFailedTransactionLog);
                    return dictionaryFailedTransactionLog;
                default:
                    return null;
            }
        }

        public void InitalBranches()
        {
            if (Branches == null)
            {
                Branches = new Dictionary<double, Branch>();
            }

            if (Branches.Count == 0)
            {
                Branches.Add(1, new Branch
                {
                    BranchId = new Random().Next(0, Int32.MaxValue),
                    BankName = "SBI",
                    IFSCCode = "SBIN03939",
                    Name = "CRPF CAMP",
                    Address = new Address
                    {
                        Address1 = "CRPF CAMP",
                        State = "Delhi",
                        Country = "India",
                        Pincode = 110072
                    }
                });
            }
        }
    }
}

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

            InitalBranches();

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

        public static Dictionary<double,T> GetTable<T>()
        {
           var obj = Activator.CreateInstance<T>();
           
            switch (obj)
            {
                case Customer customer:
                    var json = JsonConvert.SerializeObject(BMDDatabase.Customers);
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<double, T>>(json);
                    return dictionary;
                case Account account:
                    var jsonAccount = JsonConvert.SerializeObject(BMDDatabase.Accounts);
                    var dictionaryAccount = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonAccount);
                    return dictionaryAccount;
                case Branch branch:
                    var jsonBranch = JsonConvert.SerializeObject(BMDDatabase.Branches);
                    var dictionaryBranch = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonBranch);
                    return dictionaryBranch;
                case Transaction transaction:
                    var jsonTransaction = JsonConvert.SerializeObject(BMDDatabase.Transactions);
                    var dictionaryTransaction = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonTransaction);
                    return dictionaryTransaction;
                case FailedTransactionLog transactionLog:
                    var jsonFailedTransactionLog = JsonConvert.SerializeObject(BMDDatabase.FailedTransactionLog);
                    var dictionaryFailedTransactionLog = JsonConvert.DeserializeObject<Dictionary<double, T>>(jsonFailedTransactionLog);
                    return dictionaryFailedTransactionLog;
                default:
                    return null;
            }
        }

        public static T Add<T>(T entity, double id)
        {
            //var obj = Activator.CreateInstance<T>();

            switch (entity)
            {
                case Customer customer:
                    BMDDatabase.Customers.Add(id, entity as Customer); 
                    break;

                case Account account:
                    BMDDatabase.Accounts.Add(id, entity as Account);
                    break;
                case Branch branch:
                    BMDDatabase.Branches.Add(id, entity as Branch);
                    break;
                case Transaction transaction:
                    BMDDatabase.Transactions.Add(id, entity as Transaction);
                    break;
                case FailedTransactionLog transactionLog:
                    BMDDatabase.FailedTransactionLog.Add(id, entity as FailedTransactionLog);
                    break;
                default:
                    return entity;
            }

            return entity;
        }

        public static T Update<T>(T entity, double id)
        {
            //var obj = Activator.CreateInstance<T>();

            switch (entity)
            {
                case Customer customer:
                    BMDDatabase.Customers[id] = entity as Customer;
                    break;

                case Account account:
                    BMDDatabase.Accounts[id] = entity as Account;
                    break;
                case Branch branch:
                    BMDDatabase.Branches[id] =entity as Branch;
                    break;
                case Transaction transaction:
                    BMDDatabase.Transactions[id] = entity as Transaction;
                    break;
                case FailedTransactionLog transactionLog:
                    BMDDatabase.FailedTransactionLog[id] = entity as FailedTransactionLog;
                    break;
                default:
                    return entity;
            }

            return entity;
        }

        public static bool Remove<T>(double id)
        {
            var obj = Activator.CreateInstance<T>();

            switch (obj)
            {
                case Customer customer:
                    if (!BMDDatabase.Customers.ContainsKey(id))
                        return false;
                    BMDDatabase.Customers.Remove(id);
                    break;
                case Account account:
                    if (!BMDDatabase.Accounts.ContainsKey(id))
                        return false;
                    BMDDatabase.Accounts.Remove(id);
                    break;
                case Branch branch:
                    if (!BMDDatabase.Branches.ContainsKey(id))
                        return false;
                    BMDDatabase.Branches.Remove(id);
                    break;
                case Transaction transaction:
                    if (!BMDDatabase.Transactions.ContainsKey(id))
                        return false;
                    BMDDatabase.Transactions.Remove(id);
                    break;
                case FailedTransactionLog transactionLog:
                    if (!BMDDatabase.FailedTransactionLog.ContainsKey(id))
                        return false;
                    BMDDatabase.FailedTransactionLog.Remove(id);
                    break;
                default:
                    return false;
            }

            return true;
        }

        public static void InitalBranches()
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

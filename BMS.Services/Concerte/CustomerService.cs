using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Constant;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.Extensions.Logging;

namespace BMS.Services.Concerte
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IRepository<Customer> repoCustomer;
        public CustomerService(ILogger<CustomerService> logger, IRepository<Customer> repoCustomer)
        {
            _logger = logger;
            this.repoCustomer = repoCustomer;
        }
        public async Task<Customer> CreateOrGet(CustomerDetail customerDetail)
        {
            try
            {
                Customer customer = null;
                var customers = repoCustomer.GetAll();

                if (customers == null || customers.Count == 0)
                {
                    customer = await Create(customerDetail);
                    return customer;
                }

                customer = customers.Where(x => x.Value.UserName.ToLower() == customerDetail.UserName.ToLower() && x.Value.Email.ToLower() == customerDetail.Email.ToLower()).Select(x => x.Value).SingleOrDefault();
                if (customer != null)
                {
                    return await Task.FromResult(customer);
                }

                customer = await Create(customerDetail);

                return customer;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");
                throw;
            }


        }
        public async Task<Customer> Get(long Id)
        {
            try
            {
                var customer = repoCustomer.Get(Id);

                return await Task.FromResult(customer);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                throw;
            }
        }
        public async Task<bool> Delete(long Id)
        {
            try
            {
                var deleted = repoCustomer.Delete(Id);

                return await Task.FromResult(deleted);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                throw;
            }
        }

        public async Task<Customer> Create(CustomerDetail customerDetail)
        {
            try
            {
                var customers = repoCustomer.GetAll();

                var selectCustomers = customers.Where(x => x.Value.UserName.ToLower() == customerDetail.UserName.ToLower() && x.Value.Email.ToLower() == customerDetail.Email.ToLower()).Select(x => x.Value).ToList();
                if (selectCustomers.Count >= 1)
                {
                    throw new Exception(Constant.Customer_Already_Exist);
                }

                var customer = new Customer
                {
                    CustomerId = new Random().Next(0, Int32.MaxValue),
                    UserName = customerDetail.UserName,
                    Email = customerDetail.Email,
                    Password = Constant.Password,
                    Phone = customerDetail.Phone,
                    RegistrationDate = DateTime.Now,
                    Address = customerDetail.Address
                };
                var createdCustomer = repoCustomer.Insert(customer, customer.CustomerId);

                return await Task.FromResult(createdCustomer);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");
                throw;
            }
        }

        public bool ValidUser(string username, string password)
        {
            try
            {
                Customer customer = null;
                var customers = repoCustomer.GetAll();

                if (customers == null)
                {
                    return false;
                }

                customer = customers.Where(x => x.Value.UserName.ToLower() == username.ToLower() && x.Value.Password == password).Select(x => x.Value).First();
                if (customer == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");
                return false;
            }


        }
    }
}
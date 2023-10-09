using BMS.Models.Domain;
using BMS.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Interface
{
    public interface ICustomerService
    {
        Task<Customer> CreateOrGet(CustomerDetail customer);

        Task<Customer> Create(CustomerDetail customer);

        Task<Customer> Get(double Id);

        Task<bool> Delete(double Id);

        bool ValidUser(string username, string password);
    }
}

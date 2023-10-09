using BMS.Models.Domain;
using BMS.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Interface
{
    public interface IBranchService
    {
        Task<Branch> Create(BranchDetail branch);

        Task<Branch> Get(double Id);

        Task<bool> Delete(double Id);

        Task<Branch> GetByName(string branch, string bank);
    }
}

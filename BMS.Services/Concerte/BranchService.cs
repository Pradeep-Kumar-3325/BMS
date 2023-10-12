using BMS.Data.Concrete;
using BMS.Data.Interface;
using BMS.Models.Constant;
using BMS.Models.Domain;
using BMS.Models.DTO;
using BMS.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Concerte
{
    public class BranchService : IBranchService
    {
        private readonly ILogger<BranchService> _logger;
        private readonly IRepository<Branch> repoBranch;
        public BranchService(ILogger<BranchService> logger, IRepository<Branch> repoBranch)
        {
            _logger = logger;
            this.repoBranch = repoBranch;
        }
        public async Task<Branch> Create(BranchDetail branchDetail)
        {
            try
            {
                //Validate Address :- No need becaue not it assignment

                // Branch Name or IFSCCODE should come from outside as parameter
                // then on the basic of this ifsccode, fetch from table or branch source from storage 
                var branch = new Branch
                {
                    BranchId = new Random().Next(0, Int32.MaxValue),
                    BankName = branchDetail.BankName,
                    IFSCCode = (new Random().Next(0, Int32.MaxValue)).ToString(),
                    Name = branchDetail.BranchName,
                };

                var createdBranch = repoBranch.Insert(branch, branch.BranchId);

                return await Task.FromResult(createdBranch);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Create :- {ex}");
                throw;
            }
        }
        public async Task<Branch> Get(long Id)
        {
            try
            {
                var createdBranch = repoBranch.Get(Id);

                return await Task.FromResult(createdBranch);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Get :- {ex}");
                throw;
            }
        }

        public async Task<Branch> GetByName(string branch, string bank)
        {
            try
            {
                var Branches = repoBranch.GetAll();

                if (Branches == null)
                {
                    throw new Exception(Constant.No_Branch);
                }

                var branches = Branches.Where(x => x.Value.Name.ToLower() == branch.ToLower() && x.Value.BankName.ToLower() == bank.ToLower()).Select(x=> x.Value).ToList();
                if (branches.Count > 1)
                {
                    throw new Exception(Constant.Branch_Already);
                }

                return await Task.FromResult(branches.SingleOrDefault());
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
                var deleted = repoBranch.Delete(Id);

                return await Task.FromResult(deleted);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error in Delete :- {ex}");
                throw;
            }
        }
    }
}

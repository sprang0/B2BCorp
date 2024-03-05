using B2BCorp.Contracts.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerEditRA
    {
        Task<Result<Guid>> AddCustomer(string name);
        Task<Result> VerifyCustomer(Guid customerId);
        Task<Result> SetCustomerCreditLimit(Guid customerId, decimal creditLimit);
    }
}

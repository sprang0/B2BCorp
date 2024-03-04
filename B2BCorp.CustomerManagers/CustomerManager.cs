using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class CustomerManager(ICustomerRA customerRA) : ICustomerManager
    {
        readonly ICustomerRA customerRA = customerRA;

        public async Task<Result<Guid>> AddCustomer(string name)
        {
            return await customerRA.AddCustomer(name);
        }

        public async Task<Result<bool>> CustomerExists(string name)
        {
            return await customerRA.CustomerExists(name);
        }

        public async Task<Result<Guid>> GetCustomerId(string name)
        {
            return await customerRA.GetCustomerId(name);
        }

        public async Task<Result> SetCustomerCreditLimit(Guid customerId, decimal creditLimit)
        {
            return await customerRA.SetCustomerCreditLimit(customerId, creditLimit);
        }

        public async Task<Result> VerifyCustomer(Guid customerId)
        {
            return await customerRA.VerifyCustomer(customerId);
        }
    }
}

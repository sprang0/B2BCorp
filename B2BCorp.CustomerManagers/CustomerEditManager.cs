using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class CustomerEditManager(ICustomerEditRA customerEditRA) : ICustomerEditManager
    {
        readonly ICustomerEditRA customerEditRA = customerEditRA;

        public async Task<Result<Guid>> AddCustomer(string name)
        {
            return await customerEditRA.AddCustomer(name);
        }

        public async Task<Result> SetCustomerCreditLimit(Guid customerId, decimal creditLimit)
        {
            return await customerEditRA.SetCustomerCreditLimit(customerId, creditLimit);
        }

        public async Task<Result> VerifyCustomer(Guid customerId)
        {
            return await customerEditRA.VerifyCustomer(customerId);
        }
    }
}

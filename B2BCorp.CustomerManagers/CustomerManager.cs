using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class CustomerManager(ICustomerRA customerRA) : ICustomerManager
    {
        readonly ICustomerRA customerRA = customerRA;

        public async Task<Guid> AddCustomer(string name)
        {
            return await customerRA.AddCustomer(name);
        }

        public async Task<bool> CustomerExists(string name)
        {
            return await customerRA.CustomerExists(name);
        }

        public async Task<Guid> GetCustomerId(string name)
        {
            return await customerRA.GetCustomerId(name);
        }

        public async Task SetCustomerCreditLimit(Guid customerId, decimal creditLimit)
        {
            await customerRA.SetCustomerCreditLimit(customerId, creditLimit);
        }

        public async Task VerifyCustomer(Guid customerId)
        {
            await customerRA.VerifyCustomer(customerId);
        }
    }
}

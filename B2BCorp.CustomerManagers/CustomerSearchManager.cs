using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class CustomerSearchManager(ICustomerSearchRA customerRA) : ICustomerSearchManager
    {
        readonly ICustomerSearchRA customerRA = customerRA;

        public async Task<Result<bool>> CustomerExists(string name)
        {
            return await customerRA.CustomerExists(name);
        }

        public async Task<Result<Guid>> GetCustomerId(string name)
        {
            return await customerRA.GetCustomerId(name);
        }
    }
}

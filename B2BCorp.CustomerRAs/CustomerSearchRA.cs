using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.CustomerRAs
{
    public class CustomerSearchRA(B2BDbContext dbContext) : ICustomerSearchRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result<bool>> CustomerExists(string name)
        {
            var exists = await dbContext.Customers.AnyAsync(x => x.Name == name);

            return new Result<bool>(exists);
        }

        public async Task<Result<Guid>> GetCustomerId(string name)
        {
            var customer = await GetCustomerByName(name);

            return new Result<Guid>(customer.CustomerId);
        }

        #region Helpers

        private async Task<B2BDbContext.Customer> GetCustomerByName(string name)
        {
            return await dbContext.Customers
                .SingleAsync(x => x.Name == name);
        }

        #endregion
    }
}

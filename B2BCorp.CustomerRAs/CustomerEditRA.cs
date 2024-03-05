using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BCorp.CustomerRAs
{
    public class CustomerEditRA(B2BDbContext dbContext) : ICustomerEditRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result<Guid>> AddCustomer(string name)
        {
            var customer = new B2BDbContext.Customer
            {
                Name = name
            };

            dbContext.Customers.Add(customer);

            await dbContext.SaveChangesAsync();

            return new Result<Guid>(customer.CustomerId);
        }

        public async Task<Result> SetCustomerCreditLimit(Guid customerId, decimal creditLimit)
        {
            var customer = await GetCustomerById(customerId);

            customer.CreditLimit = creditLimit;

            await dbContext.SaveChangesAsync();

            return new Result();
        }

        public async Task<Result> VerifyCustomer(Guid customerId)
        {
            var customer = await GetCustomerById(customerId);

            customer.IsVerified = true;

            await dbContext.SaveChangesAsync();

            return new Result();
        }

        #region Helpers

        private async Task<B2BDbContext.Customer> GetCustomerById(Guid customerId)
        {
            return await dbContext.Customers
                .SingleAsync(x => x.CustomerId == customerId);
        }

        #endregion
    }
}

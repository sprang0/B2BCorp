using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.CustomerRAs
{
    public class CustomerValidationRA(B2BDbContext dbContext) : ICustomerValidationRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<bool> IsCustomerVerified(Guid customerId)
        {
            return (await dbContext.Customers
                .SingleAsync(x => x.CustomerId == customerId))
                .IsVerified;
        }

        public async Task<bool> IsOrderPriceAllowed(Guid customerId, Guid productId, int quantity)
        {
            var productPrice = (await dbContext.Products
                .SingleAsync(x => x.ProductId == productId)).Price;
            var productTotalPrice = productPrice * quantity;                

            var orderItemPrices = await dbContext.OrderItems
                .Where(x => x.Order.CustomerId == customerId && x.Order.OrderDateTime == null)
                .Select(x => x.TotalPrice)
                .ToListAsync();
            var orderTotalPrice = orderItemPrices.Sum();   // SQLite doesn't allow Sum on decimals

            var creditLimit = (await dbContext.Customers
                .SingleAsync(x => x.CustomerId == customerId))
                .CreditLimit;

            var unpaidTotal = await GetUnpaidTotal(customerId);

            return orderTotalPrice + productTotalPrice < creditLimit - unpaidTotal;
        }

        #region Helpers

        private async Task<decimal> GetUnpaidTotal(Guid customerId)
        {
            var invoiceAmounts = await dbContext.Invoices
                .Where(x => x.CustomerId == customerId && !x.IsPaidInFull)
                .Select(x => x.AmountDue)
                .ToListAsync();

            return invoiceAmounts.Sum();    // SQLite doesn't allow Sum on decimals 
        }

        #endregion
    }
}

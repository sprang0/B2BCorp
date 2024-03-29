﻿using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.CustomerRAs
{
    public class CustomerValidationRA(B2BDbContext dbContext) : ICustomerValidationRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result> IsCustomerVerified(Guid customerId)
        {
            var verified = (await dbContext.Customers
                .SingleAsync(x => x.CustomerId == customerId))
                .IsVerified;

            return verified ? new Result() 
                : new Result(Outcomes.ValidationError, nameof(customerId), customerId, "Customer is not verified.");
        }

        public async Task<Result> IsOrderPriceAllowed(Guid customerId, Guid productId, int quantity)
        {
            var productPrice = (await dbContext.Products
                .SingleAsync(x => x.ProductId == productId)).Price;
            var productTotalPrice = productPrice * quantity;

            var orderTotalPrice = await dbContext.OrderItems
                .Where(x => x.Order.CustomerId == customerId && x.Order.OrderDateTime == null)
                .SumAsync(x => x.TotalPrice);

            var creditLimit = (await dbContext.Customers
                .SingleAsync(x => x.CustomerId == customerId))
                .CreditLimit;

            var unpaidTotal = await GetUnpaidTotal(customerId);

            if (orderTotalPrice + productTotalPrice < creditLimit - unpaidTotal)
                return new Result();
            else
                return new Result(Outcomes.ValidationError, nameof(productId), productId,
                    $"Adding {quantity} products to order (${orderTotalPrice}) exceeds your credit limit (${creditLimit}) and unpaid orders (${unpaidTotal}).");
        }

        #region Helpers

        private async Task<decimal> GetUnpaidTotal(Guid customerId)
        {
            var invoiceAmounts = await dbContext.Invoices
                .Where(x => x.Order.CustomerId == customerId && !x.IsPaidInFull)
                .SumAsync(x => x.AmountDue);

            return invoiceAmounts;
        }

        #endregion
    }
}

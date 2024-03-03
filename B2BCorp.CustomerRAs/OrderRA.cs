using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.CustomerRAs
{
    public class OrderRA(B2BDbContext dbContext) : IOrderRA
    {
        readonly B2BDbContext dbContext = dbContext;

        const int MaxUnpaidInvoiceTotal = 50;

        public async Task AddProductToOrder(Guid customerId, Guid productId, int quantity)
        {
            var order = await EnsureActiveOrder(customerId);

            var productPrice = (await dbContext.Products
                .SingleAsync(x => x.ProductId == productId))
                .Price;

            B2BDbContext.OrderItem orderItem = new()
            {
                Order = order,
                Product = await GetProduct(productId),
                QuantityOrdered = quantity,
                ExtendedPricePerItem = productPrice,
                TotalPrice = productPrice * quantity
            };

            dbContext.OrderItems.Add(orderItem);

            await dbContext.SaveChangesAsync();
        }

        public async Task PlaceOrder(Guid customerId)
        {
            var order = await EnsureActiveOrder(customerId);

            var orderTotalPrice = await dbContext.OrderItems
                .Where(x => x.Order.CustomerId == customerId && x.Order.OrderDateTime == null)
                .SumAsync(x => x.TotalPrice);

            var unpaidInvoicesTotal = await dbContext.Invoices
                .Where(x => x.Order.CustomerId == customerId && !x.IsPaidInFull)
                .SumAsync(x => x.AmountDue);

            order.OrderDateTime = DateTime.UtcNow;
            order.TotalPrice = orderTotalPrice;
            if (unpaidInvoicesTotal > MaxUnpaidInvoiceTotal)
                order.RequiresReview = true;

            var invoice = new B2BDbContext.Invoice
            {
                AmountDue = order.TotalPrice,
                InvoiceDateTime = DateTime.UtcNow,
                Order = order
            };

            dbContext.Invoices.Add(invoice);

            await dbContext.SaveChangesAsync();
        }

        #region Helpers

        private async Task<B2BDbContext.Order> EnsureActiveOrder(Guid customerId)
        {
            // Assume one active order at a time, and OrderDateTime is set when submitted.
            var order = await dbContext.Orders
                .SingleOrDefaultAsync(x => x.CustomerId == customerId && x.OrderDateTime == null);

            if (order != null) return order;

            order = new()
            {
                Customer = await GetCustomer(customerId),
            };
            dbContext.Orders.Add(order);

            return order;
        }

        private async Task<B2BDbContext.Customer> GetCustomer(Guid customerId)
        {
            return await dbContext.Customers.SingleAsync(x => x.CustomerId == customerId);
        }

        private async Task<B2BDbContext.Product> GetProduct(Guid productId)
        {
            return await dbContext.Products.SingleAsync(x => x.ProductId == productId);
        }

        #endregion
    }
}

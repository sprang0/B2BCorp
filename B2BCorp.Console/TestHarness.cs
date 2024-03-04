using System;
using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.Managers.Product;

namespace B2BCorp.Console
{
    public class TestHarness(ICustomerManager customerManager, IProductManager productManager, IOrderManager orderManager)
    {
        readonly ICustomerManager customerManager = customerManager;
        readonly IProductManager productManager = productManager;
        readonly IOrderManager orderManager = orderManager;

        const int CustomerIdNotVerified = 1;
        const int ProductDiscontinued = 2;
        const int ProductNotActivated = 3;
        const int CreditLimit = 150;
        const int MinQuantity = 12;
        const int MaxQuantity = 144;

        readonly List<Customer> customers =
        [
            new() { Name = "Alpha Corp" },
            new() { Name = "Beta Company" },
            new() { Name = "Corporate Corporation" },
            new() { Name = "Delta Business" },
            new() { Name = "Eagle Industries" }
        ];
        readonly List<Product> products =
        [
            new() { Name = "Twinkies", Price = 1.59M },
            new() { Name = "Ho-Hos", Price = 2.19M },
            new() { Name = "Li'l Debbies", Price = 2.29M },
            new() { Name = "Snack Kakes", Price = .79M },
            new() { Name = "TastyCakes", Price = 1.99M }
        ];

        public async Task CreateData()
        {
            // Block recreating data
            if ((await customerManager.CustomerExists(customers[0].Name!)).Value) return;

            await CreateCustomers();

            await CreateProducts();

            await DiscontinueProduct();
        }

        public async Task LoadData()
        {
            foreach (var customer in customers)
                customer.CustomerId = (await customerManager.GetCustomerId(customer.Name!)).Value;

            foreach (var product in products)
                product.ProductId = (await productManager.GetProductId(product.Name!)).Value;
        }

        public async Task PlaceExcessiveOrders()
        {
            await System.Console.Out.WriteLineAsync("Trying to buy too much/little...");

            // Try to buy more than allowed
            var result = await orderManager.AddProductToOrder
                (customers[0].CustomerId, products[4].ProductId, MaxQuantity + 1);
            await OutputResult(result);

            // Try to buy less than allowed
            result = await orderManager.AddProductToOrder
                (customers[2].CustomerId, products[0].ProductId, MinQuantity - 1);
            await OutputResult(result);

            // Try to order more than credit limit allows
            result = await orderManager.AddProductToOrder
                (customers[4].CustomerId, products[1].ProductId, 50);
            await OutputResult(result);
        }

        public async Task PlaceInvalidOrders()
        {
            await System.Console.Out.WriteLineAsync("Trying to buy with unverified customers or invalid products...");

            // Unverified customer
            var result = await orderManager.AddProductToOrder
                (customers[CustomerIdNotVerified].CustomerId, products[0].ProductId, 20);
            await OutputResult(result);

            // Unactivated product
            result = await orderManager.AddProductToOrder
                (customers[0].CustomerId, products[ProductNotActivated].ProductId, 20);
            await OutputResult(result);

            // Discontinued product
            result = await orderManager.AddProductToOrder
                (customers[4].CustomerId, products[ProductDiscontinued].ProductId, 20);
            await OutputResult(result);
        }

        public async Task PlaceGoodOrder()
        {
            await System.Console.Out.WriteLineAsync("Making some (hopefully) valid purchases...");

            var customerId = customers[0].CustomerId;

            var result = await orderManager.AddProductToOrder
                (customerId, products[0].ProductId, 15);
            await OutputResult(result);

            result = await orderManager.AddProductToOrder
                (customerId, products[1].ProductId, 15);
            await OutputResult(result);

            // Running 1st time will create an order.
            // Running 2nd time will create an order requiring review.
            // Running 3rd time will not be allowed.
            if (result.Successful)
            {
                await orderManager.PlaceOrder(customerId);
                await System.Console.Out.WriteLineAsync("Order PLACED!");
            }
        }

        public async Task PlaceOrderBeyondCreditLimit()
        {
            await System.Console.Out.WriteLineAsync("Making purchases beyond credit limit...");

            var customerId = customers[2].CustomerId;

            // Running a 2nd time will exceed credit limit here
            var result = await orderManager.AddProductToOrder
                (customerId, products[0].ProductId, 35);
            await OutputResult(result);

            result = await orderManager.AddProductToOrder
                (customerId, products[1].ProductId, 35);
            await OutputResult(result);

            result = await orderManager.AddProductToOrder
                (customerId, products[4].ProductId, 35);
            await OutputResult(result);

            // Never allowed
            if (result.Successful)
            {
                await orderManager.PlaceOrder(customerId);
                throw new Exception();
            }
        }

        #region Helpers

        private async Task CreateCustomers()
        {
            await System.Console.Out.WriteLineAsync("Creating Customers...");
            foreach (var customer in customers)
            {
                if ((await customerManager.CustomerExists(customer.Name!)).Value) continue;

                customer.CustomerId = (await customerManager.AddCustomer(customer.Name!)).Value;

                // Don't verify one customer or set its credit limit
                if (customer == customers[CustomerIdNotVerified]) continue;

                await customerManager.VerifyCustomer(customer.CustomerId);
                await customerManager.SetCustomerCreditLimit(customer.CustomerId, CreditLimit);
            }
        }

        private async Task CreateProducts()
        {
            await System.Console.Out.WriteLineAsync("Creating Products...");
            foreach (var product in products)
            {
                if ((await productManager.ProductExists(product.Name!)).Value) continue;

                product.ProductId = (await productManager.AddProduct
                    (product.Name!, product.Price, MinQuantity, MaxQuantity)).Value;

                // Don't activate one product
                if (product == products[ProductNotActivated]) continue;

                await productManager.ActivateProduct(product.ProductId);
            }
        }

        private async Task DiscontinueProduct()
        {
            await System.Console.Out.WriteLineAsync("Discontinuing a Product...");
            await productManager.DiscontinueProduct(products[ProductDiscontinued].ProductId);
        }

        private static async Task OutputResult(Result result)
        {
            if (result.Successful)
                await System.Console.Out.WriteLineAsync("Order was allowed");
            else
                await System.Console.Out.WriteLineAsync($"Order was NOT allowed : " +
                    $"'{result.PropertyError[result.PropertyError.Keys.First()]} ({result.PropertyError.Keys.First()})'");
        }

        #endregion

        #region Client classes

        class Customer
        {
            public string? Name { get; set; }
            public Guid CustomerId { get; set; } = Guid.Empty;
        }

        class Product
        {
            public string? Name { get; set; }
            public decimal Price { get; set; }
            public Guid ProductId { get; set; } = Guid.Empty;
        }

        #endregion
    }
}

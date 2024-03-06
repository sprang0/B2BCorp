using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.Managers.Product;

namespace B2BCorp.Console
{
    public class TestHarness(ICustomerSearchManager customerSearchManager,
        ICustomerEditManager customerEditManager,
        IProductSearchManager productSearchManager,
        IProductEditManager productEditManager,
        ICustomerOrderManager customerOrderManager)
    {
        readonly ICustomerSearchManager customerSearchManager = customerSearchManager;
        readonly ICustomerEditManager customerEditManager = customerEditManager;
        readonly IProductSearchManager productSearchManager = productSearchManager;
        readonly IProductEditManager productEditManager = productEditManager;
        readonly ICustomerOrderManager customerOrderManager = customerOrderManager;

        const int AlphaCorpId = 0;
        const int BetaCompanyNotVerifiedId = 1;
        const int CorporateCorporationId = 2;
        const int DeltaBusinessId = 3;
        const int EagleIndustriesId = 4;

        const int TwinkiesId = 0;
        const int HoHosId = 1;
        const int LilDebbiesDiscontinuedId = 2;
        const int SnakKakesNotActivatedId = 3;
        const int TastyCakesId = 4;
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
            new() { Name = "Snak Kakes", Price = .79M },
            new() { Name = "TastyCakes", Price = 1.99M }
        ];

        public async Task CreateData()
        {
            // Block recreating data
            if ((await customerSearchManager.CustomerExists(customers[AlphaCorpId].Name!)).Value) return;

            await CreateCustomers();

            await CreateProducts();

            await DiscontinueProduct();
        }

        public async Task LoadData()
        {
            foreach (var customer in customers)
                customer.CustomerId = (await customerSearchManager.GetCustomerId(customer.Name!)).Value;

            foreach (var product in products)
                product.ProductId = (await productSearchManager.GetProductId(product.Name!)).Value;
        }

        public async Task PlaceExcessiveOrders()
        {
            await Output("Trying to buy too much/little...");

            // Try to buy more than allowed
            var result = await customerOrderManager.AddProductToOrder
                (customers[AlphaCorpId].CustomerId, products[TastyCakesId].ProductId, MaxQuantity + 1);
            await OutputOrderResult(result);

            // Try to buy less than allowed
            result = await customerOrderManager.AddProductToOrder
                (customers[CorporateCorporationId].CustomerId, products[TwinkiesId].ProductId, MinQuantity - 1);
            await OutputOrderResult(result);

            // Try to order more than credit limit allows
            result = await customerOrderManager.AddProductToOrder
                (customers[EagleIndustriesId].CustomerId, products[HoHosId].ProductId, 50);
            await OutputOrderResult(result);
        }

        public async Task PlaceInvalidOrders()
        {
            await Output("Trying to buy with unverified customers or invalid products...");

            // Unverified customer
            var result = await customerOrderManager.AddProductToOrder
                (customers[BetaCompanyNotVerifiedId].CustomerId, products[TwinkiesId].ProductId, 20);
            await OutputOrderResult(result);

            // Unactivated product
            result = await customerOrderManager.AddProductToOrder
                (customers[AlphaCorpId].CustomerId, products[SnakKakesNotActivatedId].ProductId, 20);
            await OutputOrderResult(result);

            // Discontinued product
            result = await customerOrderManager.AddProductToOrder
                (customers[EagleIndustriesId].CustomerId, products[LilDebbiesDiscontinuedId].ProductId, 20);
            await OutputOrderResult(result);
        }

        public async Task PlaceGoodOrder()
        {
            await Output("Making some (hopefully) valid purchases...");

            var customerId = customers[AlphaCorpId].CustomerId;

            var result = await customerOrderManager.AddProductToOrder
                (customerId, products[TwinkiesId].ProductId, 15);
            await OutputOrderResult(result);

            result = await customerOrderManager.AddProductToOrder
                (customerId, products[HoHosId].ProductId, 15);
            await OutputOrderResult(result);

            // Running 1st time will create an order.
            // Running 2nd time will create an order requiring review.
            // Running 3rd time will not be allowed.
            if (result.Successful)
            {
                await customerOrderManager.PlaceOrder(customerId);
                await Output("Order PLACED!");
            }
        }

        public async Task PlaceOrderBeyondCreditLimit()
        {
            await Output("Making purchases beyond credit limit...");

            var customerId = customers[CorporateCorporationId].CustomerId;

            // Running a 2nd time will exceed credit limit here
            var result = await customerOrderManager.AddProductToOrder
                (customerId, products[TwinkiesId].ProductId, 35);
            await OutputOrderResult(result);

            result = await customerOrderManager.AddProductToOrder
                (customerId, products[HoHosId].ProductId, 35);
            await OutputOrderResult(result);

            result = await customerOrderManager.AddProductToOrder
                (customerId, products[TastyCakesId].ProductId, 35);
            await OutputOrderResult(result);

            // Never allowed
            if (result.Successful)
            {
                await customerOrderManager.PlaceOrder(customerId);
                throw new Exception();
            }
        }

        public async Task UpdateProductsConcurrently()
        {
            var products = (await productSearchManager.ListAllProducts()).Value;
            var product = products.Single(x => x.Name == "Twinkies");

            var newPrice = Random.Shared.Next(150, 301) / 100M;
            await Output($"Updating price to ${newPrice}");
            product.Price = newPrice;
            var result = await productEditManager.UpdateProduct(product);
            await OutputProductUpdateResult(result);

            newPrice = Random.Shared.Next(150, 301) / 100M;
            await Output($"Updating price to ${newPrice}");
            product.Price = newPrice;
            result = await productEditManager.UpdateProduct(product);
            await OutputProductUpdateResult(result);
        }

        #region Helpers

        private async Task CreateCustomers()
        {
            await Output("Creating Customers...");

            foreach (var customer in customers)
            {
                if ((await customerSearchManager.CustomerExists(customer.Name!)).Value) continue;

                customer.CustomerId = (await customerEditManager.AddCustomer(customer.Name!)).Value;

                // Don't verify one customer or set its credit limit
                if (customer == customers[BetaCompanyNotVerifiedId]) continue;

                await customerEditManager.VerifyCustomer(customer.CustomerId);
                await customerEditManager.SetCustomerCreditLimit(customer.CustomerId, CreditLimit);
            }
        }

        private async Task CreateProducts()
        {
            await Output("Creating Products...");

            foreach (var product in products)
            {
                if ((await productSearchManager.ProductExists(product.Name!)).Value) continue;

                product.ProductId = (await productEditManager.AddProduct
                    (product.Name!, product.Price, MinQuantity, MaxQuantity)).Value;

                // Don't activate one product
                if (product == products[SnakKakesNotActivatedId]) continue;

                await productEditManager.ActivateProduct(product.ProductId);
            }
        }

        private async Task DiscontinueProduct()
        {
            await Output("Discontinuing a Product...");

            await productEditManager.DiscontinueProduct(products[LilDebbiesDiscontinuedId].ProductId);
        }

        private static async Task OutputProductUpdateResult(Result result)
        {
            if (result.Successful)
                await Output("Product update allowed");
            else if (result.ConcurrencyError)
                await Output($"Product update NOT allowed : " +
                    $"'{result.PropertyError[result.PropertyError.Keys.First()]} ({result.PropertyError.Keys.First()})'");
        }

        private static async Task OutputOrderResult(Result result)
        {
            if (result.Successful)
                await Output("Order was allowed");
            else
                await Output($"Order was NOT allowed : " +
                    $"'{result.PropertyError[result.PropertyError.Keys.First()]} ({result.PropertyError.Keys.First()})'");
        }

        private static async Task Output(string output)
        {
            await System.Console.Out.WriteLineAsync(output);
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

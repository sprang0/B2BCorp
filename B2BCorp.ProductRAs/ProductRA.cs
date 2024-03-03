using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductRA(B2BDbContext dbContext) : IProductRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Guid> AddProduct(string name, decimal price, int minAllowed, int maxAllowed)
        {
            var product = new B2BDbContext.Product
            {
                Name = name,
                Price = price,
                MinAllowedPerOrder = minAllowed,
                MaxAllowedPerOrder = maxAllowed
            };

            await dbContext.AddAsync(product);

            await dbContext.SaveChangesAsync();

            return product.ProductId;
        }

        public async Task<bool> ProductExists(string name)
        {
            return await dbContext.Products.AnyAsync(x => x.Name == name);
        }

        public async Task<Guid> GetProductId(string name)
        {
            var product = await GetProductByName(name);

            return product.ProductId;
        }

        public async Task ActivateProduct(Guid productId)
        {
            var product = await GetProductById(productId);

            product.IsActivated = true;

            dbContext.Update(product);

            await dbContext.SaveChangesAsync();
        }

        public async Task DiscontinueProduct(Guid productId)
        {
            var product = await GetProductById(productId);

            product.IsDiscontinued = true;

            dbContext.Update(product);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductResult>> ListAvailableProducts()
        {
            return await dbContext.Products
                .Where(x => x.IsActivated && !x.IsDiscontinued)
                .Select(x => new ProductResult
                {
                    ProductId = x.ProductId,
                    Name = x.Name
                })
                .ToListAsync();
        }

        #region Helpers

        private async Task<B2BDbContext.Product> GetProductById(Guid productId)
        {
            return await dbContext.Products.SingleAsync(x => x.ProductId == productId);
        }

        private async Task<B2BDbContext.Product> GetProductByName(string name)
        {
            return await dbContext.Products.SingleAsync(x => x.Name == name);
        }

        #endregion
    }
}

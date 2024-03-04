using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductRA(B2BDbContext dbContext) : IProductRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed)
        {
            var product = new B2BDbContext.Product
            {
                Name = name,
                Price = price,
                MinAllowedPerOrder = minAllowed,
                MaxAllowedPerOrder = maxAllowed
            };

            dbContext.Products.Add(product);

            await dbContext.SaveChangesAsync();

            return new Result<Guid>(product.ProductId);
        }

        public async Task<Result<bool>> ProductExists(string name)
        {
            return new Result<bool>(await dbContext.Products.AnyAsync(x => x.Name == name));
        }

        public async Task<Result<Guid>> GetProductId(string name)
        {
            var product = await GetProductByName(name);

            return new Result<Guid>(product.ProductId);
        }

        public async Task<Result> ActivateProduct(Guid productId)
        {
            var product = await GetProductById(productId);

            product.IsActivated = true;

            await dbContext.SaveChangesAsync();

            return new Result();
        }

        public async Task<Result> DiscontinueProduct(Guid productId)
        {
            var product = await GetProductById(productId);

            product.IsDiscontinued = true;

            await dbContext.SaveChangesAsync();

            return new Result();
        }

        public async Task<Result<List<ProductResult>>> ListAvailableProducts()
        {
            return new Result<List<ProductResult>>(await dbContext.Products
                .Where(x => x.IsActivated && !x.IsDiscontinued)
                .Select(x => new ProductResult
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Version = x.Version
                })
                .ToListAsync());
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

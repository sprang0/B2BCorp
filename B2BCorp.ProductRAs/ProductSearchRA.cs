using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductSearchRA(B2BDbContext dbContext) : IProductSearchRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result<bool>> ProductExists(string name)
        {
            return new Result<bool>(await dbContext.Products.AnyAsync(x => x.Name == name));
        }

        public async Task<Result<Guid>> GetProductId(string name)
        {
            var product = await GetProductByName(name);

            return new Result<Guid>(product.ProductId);
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

        private async Task<B2BDbContext.Product> GetProductByName(string name)
        {
            return await dbContext.Products.SingleAsync(x => x.Name == name);
        }

        #endregion
    }
}

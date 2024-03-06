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

        public async Task<Result<List<AvailableProduct>>> ListAvailableProducts()
        {
            var records = await dbContext.Products
                .Where(x => x.IsActivated && !x.IsDiscontinued)
                .Select(x => new AvailableProduct
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Price = x.Price
                })
                .ToListAsync();

            return new Result<List<AvailableProduct>>(records);
        }

        public async Task<Result<List<ProductDetail>>> ListAllProducts()
        {
            var records = await dbContext.Products
                .Select(x => new ProductDetail
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Price = x.Price,
                    IsActivated = x.IsActivated,
                    IsDiscontinued = x.IsDiscontinued,
                    MaxAllowedPerOrder = x.MaxAllowedPerOrder,
                    MinAllowedPerOrder = x.MinAllowedPerOrder,
                    Version = x.Version
                })
                .ToListAsync();

            return new Result<List<ProductDetail>>(records);
        }

        #region Helpers

        private async Task<B2BDbContext.Product> GetProductByName(string name)
        {
            return await dbContext.Products.SingleAsync(x => x.Name == name);
        }

        #endregion
    }
}

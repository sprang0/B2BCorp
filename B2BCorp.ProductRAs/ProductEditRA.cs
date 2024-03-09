using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductEditRA(B2BDbContext dbContext) : IProductEditRA
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

        public async Task<Result> UpdateProduct(ProductDetail productDetail)
        {
            var record = await dbContext.Products
                .SingleAsync(x => x.ProductId == productDetail.ProductId);

            record.Name = productDetail.Name;
            record.Price = productDetail.Price;
            record.MinAllowedPerOrder = productDetail.MinAllowedPerOrder;
            record.MaxAllowedPerOrder = productDetail.MaxAllowedPerOrder;
            record.IsActivated = productDetail.IsActivated;
            record.IsDiscontinued = productDetail.IsDiscontinued;

            // Assert concurrency check
            dbContext.Entry(record).Property("Version").OriginalValue = productDetail.Version;

            try
            {
                await dbContext.SaveChangesAsync();

                return new Result();
            }
            catch (DbUpdateConcurrencyException)
            {
                return new Result(Outcomes.ConcurrencyError, nameof(productDetail.ProductId), productDetail.ProductId,
                    "This product was edited while you were working with it.");
            }
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

        #region Helpers

        private async Task<B2BDbContext.Product> GetProductById(Guid productId)
        {
            return await dbContext.Products.SingleAsync(x => x.ProductId == productId);
        }

        #endregion
    }
}

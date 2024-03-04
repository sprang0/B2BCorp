using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductValidationRA(B2BDbContext dbContext) : IProductValidationRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<Result> IsProductAvailable(Guid productId)
        {
            var product = await dbContext.Products
                .Where(x => x.ProductId == productId)
                .Select(x => new { x.IsActivated, x.IsDiscontinued })
                .SingleAsync(); ;

            if (product.IsActivated && !product.IsDiscontinued)
                return new Result();
            else if (!product.IsActivated)
                return new Result(Outcomes.ValidationError, nameof(productId), productId, "Product is not activated.");
            else 
                return new Result(Outcomes.ValidationError, nameof(productId), productId, "Product is discontinued.");
        }

        public async Task<Result> IsQuantityValid(Guid productId, int quantity)
        {
            var valid = await dbContext.Products.AnyAsync(x => x.ProductId == productId
                && quantity >= x.MinAllowedPerOrder && quantity <= x.MaxAllowedPerOrder);

            return valid ? new Result()
                : new Result(Outcomes.ValidationError, nameof(productId), productId, $"Cannot order {quantity} products.");
        }
    }
}

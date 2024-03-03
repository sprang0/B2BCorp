using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace B2BCorp.ProductRAs
{
    public class ProductValidationRA(B2BDbContext dbContext) : IProductValidationRA
    {
        readonly B2BDbContext dbContext = dbContext;

        public async Task<bool> IsProductAvailable(Guid productId, int quantity)
        {
            return await dbContext.Products.AnyAsync(x => x.ProductId == productId
                && x.IsActivated && !x.IsDiscontinued);
        }

        public async Task<bool> IsQuantityValid(Guid productId, int quantity)
        {
            return await dbContext.Products.AnyAsync(x => x.ProductId == productId
                && quantity >= x.MinAllowedPerOrder && quantity <= x.MaxAllowedPerOrder);
        }
    }
}

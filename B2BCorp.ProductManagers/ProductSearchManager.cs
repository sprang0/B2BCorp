using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.Managers.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;

namespace B2BCorp.ProductManagers
{
    public class ProductSearchManager(IProductSearchRA productSearchRA) : IProductSearchManager
    {
        readonly IProductSearchRA productSearchRA = productSearchRA;

        public async Task<Result<bool>> ProductExists(string name)
        {
            return await productSearchRA.ProductExists(name);
        }

        public async Task<Result<Guid>> GetProductId(string name)
        {
            return await productSearchRA.GetProductId(name);
        }

        public async Task<Result<List<AvailableProduct>>> ListAvailableProducts()
        {
            return await productSearchRA.ListAvailableProducts();
        }

        public async Task<Result<List<ProductDetail>>> ListAllProducts()
        {
            return await productSearchRA.ListAllProducts();
        }
    }
}

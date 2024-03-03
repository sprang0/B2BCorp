using B2BCorp.Contracts.DTOs.Product;
using B2BCorp.Contracts.Managers.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;

namespace B2BCorp.ProductManagers
{
    public class ProductManager(IProductRA productRA) : IProductManager
    {
        readonly IProductRA productRA = productRA;

        public async Task<Guid> AddProduct(string name, decimal price, int minAllowed, int maxAllowed)
        {
            return await productRA.AddProduct(name, price, minAllowed, maxAllowed);
        }

        public async Task<bool> ProductExists(string name)
        {
            return await productRA.ProductExists(name);
        }

        public async Task<Guid> GetProductId(string name)
        {
            return await productRA.GetProductId(name);
        }

        public async Task ActivateProduct(Guid productId)
        {
            await productRA.ActivateProduct(productId);
        }

        public async Task DiscontinueProduct(Guid productId)
        {
            await productRA.DiscontinueProduct(productId);
        }

        public async Task<List<ProductResult>> ListAvailableProducts()
        {
            return await productRA.ListAvailableProducts();
        }
    }
}

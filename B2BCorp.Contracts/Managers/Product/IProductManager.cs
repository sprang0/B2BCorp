using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.Managers.Product
{
    public interface IProductManager
    {
        public Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        public Task<Result<bool>> ProductExists(string name);
        public Task<Result<Guid>> GetProductId(string name);
        public Task<Result<List<DTOs.Product.ProductResult>>> ListAvailableProducts();
        public Task<Result> ActivateProduct(Guid productId);
        public Task<Result> DiscontinueProduct(Guid productId);
    }
}

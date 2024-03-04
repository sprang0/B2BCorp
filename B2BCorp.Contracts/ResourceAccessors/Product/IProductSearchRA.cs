using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductSearchRA
    {
        public Task<Result<bool>> ProductExists(string name);
        public Task<Result<Guid>> GetProductId(string name);
        public Task<Result<List<DTOs.Product.ProductResult>>> ListAvailableProducts();
    }
}

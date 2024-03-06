using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;

namespace B2BCorp.Contracts.Managers.Product
{
    public interface IProductSearchManager
    {
        Task<Result<bool>> ProductExists(string name);
        Task<Result<Guid>> GetProductId(string name);
        Task<Result<List<DTOs.Product.AvailableProduct>>> ListAvailableProducts();
        Task<Result<List<ProductDetail>>> ListAllProducts();
    }
}

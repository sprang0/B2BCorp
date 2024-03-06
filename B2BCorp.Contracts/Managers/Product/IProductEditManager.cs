using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;

namespace B2BCorp.Contracts.Managers.Product
{
    public interface IProductEditManager
    {
        Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        Task<Result> UpdateProduct(ProductDetail productDetail);
        Task<Result> ActivateProduct(Guid productId);
        Task<Result> DiscontinueProduct(Guid productId);
    }
}

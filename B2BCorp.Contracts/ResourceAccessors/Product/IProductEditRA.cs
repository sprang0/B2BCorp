using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.DTOs.Product;

namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductEditRA
    {
        Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        Task<Result> ActivateProduct(Guid productId);
        Task<Result> DiscontinueProduct(Guid productId);
        Task<Result> UpdateProduct(ProductDetail productDetail);
    }
}

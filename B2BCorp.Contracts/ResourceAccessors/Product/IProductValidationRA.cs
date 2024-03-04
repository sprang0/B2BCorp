using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductValidationRA
    {
        Task<Result> IsQuantityValid(Guid productId, int quantity);
        Task<Result> IsProductAvailable(Guid productId);
    }
}

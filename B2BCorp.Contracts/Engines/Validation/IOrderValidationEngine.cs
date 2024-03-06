using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.Engines.Validation
{
    public interface IOrderValidationEngine
    {
        Task<Result> ValidateOrder(Guid customerId, Guid productId, int quantity);
    }
}

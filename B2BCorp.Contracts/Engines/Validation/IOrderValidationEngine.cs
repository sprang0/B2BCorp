using B2BCorp.Contracts.DTOs.Customer;

namespace B2BCorp.Contracts.Engines.Validation
{
    public interface IOrderValidationEngine
    {
        public Task<OrderValidationResult> ValidateOrder(Guid customerId, Guid productId, int quantity);
    }
}

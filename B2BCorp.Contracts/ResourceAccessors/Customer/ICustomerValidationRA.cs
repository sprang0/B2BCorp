using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerValidationRA
    {
        public Task<Result> IsCustomerVerified(Guid customerId);
        public Task<Result> IsOrderPriceAllowed(Guid customerId, Guid productId, int quantity);
    }
}

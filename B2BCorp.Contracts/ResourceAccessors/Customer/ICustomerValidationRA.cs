using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerValidationRA
    {
       Task<Result> IsCustomerVerified(Guid customerId);
       Task<Result> IsOrderPriceAllowed(Guid customerId, Guid productId, int quantity);
    }
}

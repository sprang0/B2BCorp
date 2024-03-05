using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerOrderRA
    {
        Task<Result> AddProductToOrder(Guid customerId, Guid productId, int quantity);
        Task<Result> PlaceOrder(Guid customerId);
    }
}

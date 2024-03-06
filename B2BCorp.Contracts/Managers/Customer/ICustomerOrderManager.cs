using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.Managers.Customer
{
    public interface ICustomerOrderManager
    {
        Task<Result> AddProductToOrder(Guid customerId, Guid productId, int quantity);
        Task<Result> PlaceOrder(Guid customerId);
    }
}

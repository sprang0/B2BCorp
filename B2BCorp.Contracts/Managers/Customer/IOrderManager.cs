using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.Managers.Customer
{
    public interface IOrderManager
    {
        public Task<Result> AddProductToOrder(Guid customerId, Guid productId, int quantity);
        public Task<Result> PlaceOrder(Guid customerId);
    }
}

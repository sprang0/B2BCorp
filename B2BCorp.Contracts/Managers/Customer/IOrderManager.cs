namespace B2BCorp.Contracts.Managers.Customer
{
    public interface IOrderManager
    {
        public Task<bool> AddProductToOrder(Guid customerId, Guid productId, int quantity);
        public Task PlaceOrder(Guid customerId);
    }
}

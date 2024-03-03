namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface IOrderRA
    {
        Task AddProductToOrder(Guid customerId, Guid productId, int quantity);
        public Task PlaceOrder(Guid customerId);
    }
}

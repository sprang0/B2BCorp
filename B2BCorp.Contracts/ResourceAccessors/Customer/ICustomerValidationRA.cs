namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerValidationRA
    {
        public Task<bool> IsCustomerVerified(Guid customerId);
        public Task<bool> IsOrderPriceAllowed(Guid customerId, Guid productId, int quantity);
    }
}

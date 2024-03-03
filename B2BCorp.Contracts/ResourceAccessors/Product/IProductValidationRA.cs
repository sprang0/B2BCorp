namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductValidationRA
    {
        Task<bool> IsQuantityValid(Guid productId, int quantity);
        Task<bool> IsProductAvailable(Guid productId, int quantity);
    }
}

namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductRA
    {
        public Task<Guid> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        public Task<bool> ProductExists(string name);
        public Task<Guid> GetProductId(string name);
        public Task ActivateProduct(Guid productId);
        public Task DiscontinueProduct(Guid productId);
        public Task<List<DTOs.Product.ProductResult>> ListAvailableProducts();
    }
}

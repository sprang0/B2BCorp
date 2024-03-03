namespace B2BCorp.Contracts.Managers.Product
{
    public interface IProductManager
    {
        public Task<Guid> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        public Task<bool> ProductExists(string name);
        public Task<Guid> GetProductId(string name);
        public Task<List<DTOs.Product.ProductResult>> ListAvailableProducts();
        public Task ActivateProduct(Guid productId);
        public Task DiscontinueProduct(Guid productId);
    }
}

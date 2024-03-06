namespace B2BCorp.Contracts.DTOs.Product
{
    public class AvailableProduct
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }
}

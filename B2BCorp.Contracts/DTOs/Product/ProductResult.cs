namespace B2BCorp.Contracts.DTOs.Product
{
    public class ProductResult
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = "";
        public byte[] Version { get; set; } = [];
    }
}

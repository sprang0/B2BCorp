namespace B2BCorp.Contracts.DTOs.Product
{
    public class ProductDetail
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDiscontinued { get; set; }
        public int MinAllowedPerOrder { get; set; }
        public int MaxAllowedPerOrder { get; set; }
        public byte[] Version { get; set; } = [];
    }
}

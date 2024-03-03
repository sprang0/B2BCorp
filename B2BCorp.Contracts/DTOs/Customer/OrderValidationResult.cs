namespace B2BCorp.Contracts.DTOs.Customer
{
    public class OrderValidationResult
    {
        public bool IsValid { get; set; }
        public bool RequiresReview { get; set; }
    }
}

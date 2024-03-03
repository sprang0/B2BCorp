namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerRA
    {
        Task<Guid> AddCustomer(string name);
        Task<Guid> GetCustomerId(string name);
        Task<bool> CustomerExists(string name);
        Task VerifyCustomer(Guid customerId);
        Task SetCustomerCreditLimit(Guid customerId, decimal creditLimit);
    }
}

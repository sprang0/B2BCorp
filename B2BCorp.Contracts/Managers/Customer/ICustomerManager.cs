namespace B2BCorp.Contracts.Managers.Customer
{
    public interface ICustomerManager
    {
        Task<Guid> AddCustomer(string name);
        Task<Guid> GetCustomerId(string name);
        Task<bool> CustomerExists(string name); 
        Task VerifyCustomer(Guid customerId);
        Task SetCustomerCreditLimit(Guid customerId, decimal creditLimit);
    }
}

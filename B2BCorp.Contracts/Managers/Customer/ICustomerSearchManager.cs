using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.Managers.Customer
{
    public interface ICustomerSearchManager
    {
        Task<Result<bool>> CustomerExists(string name);
        Task<Result<Guid>> GetCustomerId(string name);
    }
}

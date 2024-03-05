using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerSearchRA
    {
        Task<Result<Guid>> GetCustomerId(string name);
        Task<Result<bool>> CustomerExists(string name);
    }
}

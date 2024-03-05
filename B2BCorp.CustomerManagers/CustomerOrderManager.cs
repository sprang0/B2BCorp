using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Engines.Validation;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class CustomerOrderManager(IOrderValidationEngine orderValidationEngine, ICustomerOrderRA orderRA) : ICustomerOrderManager
    {
        readonly IOrderValidationEngine orderValidationEngine = orderValidationEngine;
        readonly ICustomerOrderRA orderRA = orderRA;

        public async Task<Result> AddProductToOrder(Guid customerId, Guid productId, int quantity)
        {
            var result = await orderValidationEngine.ValidateOrder(customerId, productId, quantity);

            if (!result.Successful) return result;

            result = await orderRA.AddProductToOrder(customerId, productId, quantity);

            return result;
        }

        public async Task<Result> PlaceOrder(Guid customerId)
        {
            return await orderRA.PlaceOrder(customerId);
        }
    }
}

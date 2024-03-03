using B2BCorp.Contracts.Engines.Validation;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.ResourceAccessors.Customer;

namespace B2BCorp.CustomerManagers
{
    public class OrderManager(IOrderValidationEngine orderValidationEngine, IOrderRA orderRA) : IOrderManager
    {
        readonly IOrderValidationEngine orderValidationEngine = orderValidationEngine;
        readonly IOrderRA orderRA = orderRA;

        public async Task<bool> AddProductToOrder(Guid customerId, Guid productId, int quantity)
        {
            var validation = await orderValidationEngine.ValidateOrder(customerId, productId, quantity);

            if (!validation.IsValid) return false;

            await orderRA.AddProductToOrder(customerId, productId, quantity);

            return true;
        }

        public async Task PlaceOrder(Guid customerId)
        {
            await orderRA.PlaceOrder(customerId);
        }
    }
}

using B2BCorp.Contracts.DTOs.Customer;
using B2BCorp.Contracts.Engines.Validation;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.Contracts.ResourceAccessors.Product;

namespace B2BCorp.ValidationEngines
{
    public class OrderValidationEngine(ICustomerValidationRA customerValidationRA, IProductValidationRA productValidationRA) : IOrderValidationEngine
    {
        readonly ICustomerValidationRA customerValidationRA = customerValidationRA;
        readonly IProductValidationRA productValidationRA = productValidationRA;

        public async Task<OrderValidationResult> ValidateOrder(Guid customerId, Guid productId, int quantity)
        {
            var orderValidation = new OrderValidationResult();

            if (!await customerValidationRA.IsCustomerVerified(customerId)) return orderValidation;
            if (!await productValidationRA.IsQuantityValid(productId, quantity)) return orderValidation;
            if (!await productValidationRA.IsProductAvailable(productId, quantity)) return orderValidation;
            if (!await customerValidationRA.IsOrderPriceAllowed(customerId, productId, quantity)) return orderValidation;

            orderValidation.IsValid = true;

            return orderValidation;
        }
    }
}

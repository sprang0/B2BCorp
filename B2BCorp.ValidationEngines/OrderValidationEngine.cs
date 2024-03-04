using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Engines.Validation;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.Contracts.ResourceAccessors.Product;

namespace B2BCorp.ValidationEngines
{
    public class OrderValidationEngine(ICustomerValidationRA customerValidationRA, IProductValidationRA productValidationRA) : IOrderValidationEngine
    {
        readonly ICustomerValidationRA customerValidationRA = customerValidationRA;
        readonly IProductValidationRA productValidationRA = productValidationRA;

        public async Task<Result> ValidateOrder(Guid customerId, Guid productId, int quantity)
        {
            var result = await customerValidationRA.IsCustomerVerified(customerId);
            if (!result.Successful) return result;

            result = await productValidationRA.IsProductAvailable(productId);
            if (!result.Successful) return result;

            result = await productValidationRA.IsQuantityValid(productId, quantity);
            if (!result.Successful) return result;

            result = await customerValidationRA.IsOrderPriceAllowed(customerId, productId, quantity);
            if (!result.Successful) return result;

            return new Result();
        }
    }
}
